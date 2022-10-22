using DataAccess;
using HtmlAgilityPack;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Model.BaseModel;
using Model.ViewModel;
using Nest;
using Newtonsoft.Json;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ultilities;



namespace Service.Implementation
{
    public class ScoreService : IScoreService
    {
        private readonly IApplicationSettings _appSettings;
        private readonly string _connectString;
        private readonly IElasticSearchService _elasticSearchService;
        protected readonly IElasticClient _elasticClient;
        private readonly IConfiguration _config;
        private CheckDatetimeValid validDatetime = new CheckDatetimeValid();
        private static readonly string[] VietnameseSigns = new string[]
{"aAeEoOuUiIdDyY","áàạảãâấầậẩẫăắằặẳẵ","ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ","éèẹẻẽêếềệểễ","ÉÈẸẺẼÊẾỀỆỂỄ","óòọỏõôốồộổỗơớờợởỡ","ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ","úùụủũưứừựửữ","ÚÙỤỦŨƯỨỪỰỬỮ","íìịỉĩ","ÍÌỊỈĨ","đ","Đ","ýỳỵỷỹ","ÝỲỴỶỸ"};

        public ScoreService(IApplicationSettings appSettings, IElasticSearchService elasticSearchService, IConfiguration config,IElasticClient elasticClient)
        {
            _appSettings = appSettings;
            _connectString = _appSettings.ConnectionString;
            _elasticSearchService = elasticSearchService;
            _elasticClient = elasticClient;
            _config = config;
        }

        #region Crawl
        public async Task<int> CrawlInformationUniversity()
        {
            try
            {
                int result = 0;
                List<University> listUni = new List<University>();
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    var url = "https://tuyensinhso.vn/school/dai-hoc-gia-dinh.html";
                    HtmlWeb web = new HtmlWeb();
                    var client = new HttpClient();
                    HtmlDocument htmlDoc = new HtmlDocument();
                    var response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Error occurred at {MethodBase.GetCurrentMethod().ReflectedType.Name}");
                        return -1;
                    }
                    var body = await response.Content.ReadAsStringAsync();
                    htmlDoc.LoadHtml(body);
                    HtmlNode Information = htmlDoc.DocumentNode.SelectNodes("//div[@class='detail-content']//ul")[0];
                    University university = new University();
                    university.Code = Information.ChildNodes[1].InnerText;
                    result = await uow.dbScoreRepository.CustomizeNameMajorToCode();
                    uow.Commit();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CrawlListUniversity()
        {
            try
            {
                int result = 0;
                List<University> listUni = new List<University>();
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    var result_province = await uow.dbScoreRepository.GetListProvince();
                    var result_type_university = await uow.dbScoreRepository.GetListTypeUniversity();
                    var province = await result_province.ReadAsync<Province>();
                    var type_university = await result_type_university.ReadAsync<University_Type>();
                    foreach (var item in province)
                    {
                        foreach (var item1 in type_university)
                        {
                            var url = "https://diemthi.tuyensinh247.com/danh-sach-truong-dai-hoc-cao-dang.html?level="+ item1.IdType+ "&city=" + item.IdProvince;
                            HtmlWeb web = new HtmlWeb();
                            var client = new HttpClient();
                            HtmlDocument htmlDoc = new HtmlDocument();
                            var response = await client.GetAsync(url);
                            if (!response.IsSuccessStatusCode)
                            {
                                Console.WriteLine($"Error occurred at {MethodBase.GetCurrentMethod().ReflectedType.Name}");
                                return -1;
                            }
                            var body = await response.Content.ReadAsStringAsync();
                            htmlDoc.LoadHtml(body);
                            HtmlNode tbody = htmlDoc.DocumentNode.SelectNodes("//div[@class='code-shol col-513']//table")[0];
                            int n = 3;
                            for (; ; )
                            {
                                try
                                {
                                    University uni = new University();
                                    uni.Code = tbody.ChildNodes[n].ChildNodes[1].InnerText;
                                    uni.Name = tbody.ChildNodes[n].ChildNodes[3].InnerText;
                                    uni.Province = item.IdProvince;
                                    uni.Type = item1.IdType;
                                    uni.CreatedBy = "bot";
                                    uni.CreatedDay = DateTime.Now;
                                    uni.IsDeleted = 0;
                                    listUni.Add(uni);
                                    n = n + 2;
                                }
                                catch (Exception ex)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    if(listUni != null && listUni.Count > 0)
                    {
                        string jsonUniResult = JsonConvert.SerializeObject(listUni);
                        result = await uow.dbScoreRepository.ImportUniversity(jsonUniResult);
                    }
                    uow.Commit();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckCharacter(string a)
        {
            for (int i = 0; i < 10; i++)
            {
                if(a == i.ToString())
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<int> CrawlListMajorAndCombination()
        {
            try
            {
                int result = 0;
                List<Major_Combination> lstMajor = new List<Major_Combination>();
                var lsHref = File.ReadAllLines("ListHref/ListMajorHref.txt").ToList();
                int id = 1242;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    foreach (var href in lsHref)
                    {
                        var url = href;
                        HtmlWeb web = new HtmlWeb();
                        var client = new HttpClient();
                        HtmlDocument htmlDoc = new HtmlDocument();
                        var response = await client.GetAsync(url);
                        if (!response.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"Error occurred at {MethodBase.GetCurrentMethod().ReflectedType.Name}");
                            return -1;
                        }
                        var body = await response.Content.ReadAsStringAsync();
                        htmlDoc.LoadHtml(body);
                        bool isDone = false;
                        HtmlNode nameMajor = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='detail_title']");
                        int i = 0;
                        do
                        {
                            try
                            {
                                HtmlNode combinationMajor = htmlDoc.DocumentNode.SelectNodes("//ul")[i];
                                if (CheckCharacter(combinationMajor.ChildNodes[1].InnerText.Substring(2, 1)) == true)
                                {
                                    isDone = true;                
                                    string name = nameMajor.InnerText;
                                    for (int j = 1; ;)
                                    {
                                        try
                                        {
                                            string com = combinationMajor.ChildNodes[j].InnerText.Substring(0, 3);
                                            Major_Combination mjb = new Major_Combination();
                                            id++;
                                            mjb.IdMajor_Combination = id;
                                            mjb.CodeMajor = name;
                                            mjb.CodeCombination = com;
                                            mjb.CreatedBy = "bot";
                                            mjb.CreatedDay = DateTime.Now;
                                            mjb.IsDeleted = 0;
                                            lstMajor.Add(mjb);
                                            j = j + 2;
                                        }
                                        catch (Exception ex)
                                        {
                                            break;
                                        }
                                    }
                                }
                                i++;
                                if (i == 6)
                                {
                                    isDone = true;
                                }
                            }
                            catch
                            {
                                break;
                            }
                        } while (isDone == false);
                    }
                    if (lstMajor != null && lstMajor.Count > 0)
                    {
                        string jsonMajorCombiResult = JsonConvert.SerializeObject(lstMajor);
                        result = await uow.dbScoreRepository.ImportMajor_Combination(jsonMajorCombiResult);                 
                    }
                    result = await uow.dbScoreRepository.CustomizeNameMajorToCode();
                    uow.Commit();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CrawlListMajor()
        {
            try
            {
                int result = 0;
                List<Major_Combination> lstMajor = new List<Major_Combination>();
                var lsHref = File.ReadAllLines("ListHref/ListMajorHref.txt").ToList();
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    foreach (var href in lsHref)
                    {
                        var url = href;
                        HtmlWeb web = new HtmlWeb();
                        var client = new HttpClient();
                        HtmlDocument htmlDoc = new HtmlDocument();
                        var response = await client.GetAsync(url);
                        if (!response.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"Error occurred at {MethodBase.GetCurrentMethod().ReflectedType.Name}");
                            return -1;
                        }
                        var body = await response.Content.ReadAsStringAsync();
                        htmlDoc.LoadHtml(body);
                        HtmlNode nameMajor = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='detail_title']");
                        bool isDone = false;
                        int i = 0;
                        do
                        {
                            try
                            {
                                HtmlNode detailMajor = htmlDoc.DocumentNode.SelectNodes("//p[@style='text-align: justify;']")[i];
                                if (detailMajor.InnerText.Contains("M&atilde; ng&agrave;nh"))
                                {
                                    isDone = true;
                                    string name = nameMajor.InnerText;
                                    string code = detailMajor.InnerText.Trim().Substring(detailMajor.InnerText.Trim().Length-7,7);
                                    Major mj = new Major();
                                    mj.NameMajor = name;
                                    mj.CodeMajor = code;
                                    mj.CreatedBy = "bot";
                                    mj.CreatedDay = DateTime.Now;
                                    mj.IsDeleted = 0;
                                    if (mj != null)
                                    {
                                        string jsonResultMajor = JsonConvert.SerializeObject(mj);
                                        result = await uow.dbScoreRepository.ImportMajor(jsonResultMajor);
                                    }                                  
                                }                                
                                i++;
                                if (i == 4)
                                {
                                    isDone = true;
                                }
                            }
                            catch
                            {
                                break;
                            }
                        } while (isDone == false);
                    }
                    uow.Commit();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public async Task<int> CrawlCombination()
        {
            try
            {
                int result = 0;
                List<Combination> lstCombination = new List<Combination>();
                using(UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    var url = "http://tuyensinh.dhsphue.edu.vn/Modules/nganhhoc/front_list_tohop.aspx";
                    HtmlWeb web = new HtmlWeb();
                    var client = new HttpClient();
                    HtmlDocument htmlDoc = new HtmlDocument();
                    var response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Error occurred at {MethodBase.GetCurrentMethod().ReflectedType.Name}");
                        return -1;
                    }
                    var body = await response.Content.ReadAsStringAsync();
                    htmlDoc.LoadHtml(body);
                    HtmlNode tbody = htmlDoc.DocumentNode.SelectNodes("//div[@class=' table table-hover']//table")[0];
                    int n = 3;
                    for (; ; )
                    {
                        try
                        {
                            Combination cb = new Combination();
                            cb.CodeCombination = tbody.ChildNodes[n].ChildNodes[3].InnerText;
                            cb.NameCombination = tbody.ChildNodes[n].ChildNodes[5].InnerText;
                            cb.CreatedBy = "bot";
                            cb.CreatedDay = DateTime.Now;
                            cb.IsDeleted = 0;
                            lstCombination.Add(cb);
                            n = n + 2;
                        }
                        catch (Exception ex)
                        {
                            break;
                        }
                    }
                    if (lstCombination != null && lstCombination.Count > 0)
                    {
                        string jsonUniResult = JsonConvert.SerializeObject(lstCombination);
                        result = await uow.dbScoreRepository.ImportCombination(jsonUniResult);
                    }
                    uow.Commit();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CrawlScoreUniversity(string year)
        {
            try
            {           
                int result = 0;
                List<University_Score> lstScore = new List<University_Score>();
                var lsHref = File.ReadAllLines("ListHref/ListScoreHref.txt").ToList();
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    foreach (var href in lsHref)
                    {
                        string codeUniversity = href.Substring(href.Length - 8 , 3);
                        var url = "https://diemthi.tuyensinh247.com"+href+"?y="+year;
                        HtmlWeb web = new HtmlWeb();
                        var client = new HttpClient();
                        HtmlDocument htmlDoc = new HtmlDocument();
                        var response = await client.GetAsync(url);
                        if (!response.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"Error occurred at {MethodBase.GetCurrentMethod().ReflectedType.Name}");
                            return -1;
                        }
                        var body = await response.Content.ReadAsStringAsync();
                        htmlDoc.LoadHtml(body);
                        try
                        {
                            HtmlNode tbody = htmlDoc.DocumentNode.SelectNodes("//div[@class='tab active']//table")[0];
                            int n = 3;
                            for (; ; )
                            {
                                try
                                {
                                    University_Score us = new University_Score();                                 
                                    us.CodeUniversity = codeUniversity;
                                    us.Description = tbody.ChildNodes[n].ChildNodes[11].InnerText;
                                    us.CreatedBy = "bot";
                                    us.CreatedDay = DateTime.Now;
                                    us.IsDeleted = 0;
                                    us.Score = double.Parse(tbody.ChildNodes[n].ChildNodes[9].InnerText,CultureInfo.InvariantCulture);
                                    us.CodeMajor = tbody.ChildNodes[n].ChildNodes[3].InnerText;
                                    us.NameMajor = tbody.ChildNodes[n].ChildNodes[5].InnerText;
                                    us.CodeCombination = tbody.ChildNodes[n].ChildNodes[7].InnerText;
                                    us.Year = year;
                                    lstScore.Add(us);
                                    n = n + 2;
                                }
                                catch (Exception ex)
                                {
                                    break;
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    if (lstScore != null && lstScore.Count > 0)
                    {
                        string jsonUniResult = JsonConvert.SerializeObject(lstScore);
                        result = await uow.dbScoreRepository.ImportScore(jsonUniResult);                  
                    }
                   
                    uow.Commit();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CrawlListHrefScore()
        {
            try
            {
                List<string> lsHref = new List<string>();
                int result = 0;
                var url = "https://diemthi.tuyensinh247.com";
                HtmlWeb web = new HtmlWeb();
                var client = new HttpClient();
                HtmlDocument htmlDoc = new HtmlDocument();
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error occurred at {MethodBase.GetCurrentMethod().ReflectedType.Name}");
                    return -1;
                }
                var body = await response.Content.ReadAsStringAsync();
                htmlDoc.LoadHtml(body);
                HtmlNode table_href = htmlDoc.DocumentNode.SelectNodes("//div[@class='list-schol fl']//ul")[0];
                for (int j = 0; ;)
                {
                        try
                        {
                            string item = table_href.SelectNodes(".//a")[j].Attributes["href"].Value;
                            lsHref.Add(item);
                            j++;
                        }
                        catch
                        {
                            break;
                        }
                }
                File.WriteAllLines("ListHref/ListScoreHref.txt", lsHref);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CrawlListHrefMajor()
        {
            try
            {
                List<string> lsHref = new List<string>();
                int result = 0;
                var url = "https://tuyensinhso.vn/nhom-nganh-dao-tao.html";
                HtmlWeb web = new HtmlWeb();
                var client = new HttpClient();
                HtmlDocument htmlDoc = new HtmlDocument();
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                        Console.WriteLine($"Error occurred at {MethodBase.GetCurrentMethod().ReflectedType.Name}");
                        return -1;
                }
                var body = await response.Content.ReadAsStringAsync();
                htmlDoc.LoadHtml(body);
                for (int i = 0; ;)
                {
                   try
                   {
                       HtmlNode table_href = htmlDoc.DocumentNode.SelectNodes("//div[@class='list-news list-group']")[i];
                       for (int j = 0; ;)
                       {
                            try
                            {
                                string item = table_href.SelectNodes(".//a")[j].Attributes["href"].Value;
                                lsHref.Add(item);
                                j++;
                            }
                            catch
                            {
                                break;
                            }
                       }
                       i++;
                   }
                   catch
                   {
                            break;
                   }
                }
                File.WriteAllLines("ListHref/ListMajorHref.txt", lsHref);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region GetValue
        private static string filterString(string s)
        {
            try
            {
                Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
                string temp = s.Normalize(NormalizationForm.FormD);
                return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').Trim().ToLower();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ViewUniversity>> GetUniversity()
        {
            try
            {
                IEnumerable<ViewUniversity> lstUniversity = null;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    var result = await uow.dbScoreRepository.GetListUniversity();
                    var University = await result.ReadAsync<ViewUniversity>();
                    lstUniversity = University.Select(p => new ViewUniversity
                    {
                        IdUniversity = p.IdUniversity,
                        Code = p.Code.Trim(),
                        Name = p.Name,
                        IdProvince = p.IdProvince,
                        NameProvince = p.NameProvince,
                        IdType = p.IdType,
                        NameType = p.NameType,
                        Slug = StringHelpers.GenerateSlug(p.Name)
                    }).OrderByDescending(p => p.IdType).ThenBy(p => p.Code);
                    if (lstUniversity != null)
                    {
                        return lstUniversity;
                    }
                }
                return null;
            }           
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ViewUniversity>> GetUniversityByCodeOrName(string keyword)
        {
            try
            {
                IEnumerable<ViewUniversity> lstUniversity = null;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    keyword = filterString(keyword);
                    var result = await uow.dbScoreRepository.GetListUniversity();
                    var University = await result.ReadAsync<ViewUniversity>();
                    lstUniversity = University.Select(p => new ViewUniversity
                    {
                        IdUniversity = p.IdUniversity,
                        Code = p.Code,
                        Name = p.Name,
                        IdProvince = p.IdProvince,
                        NameProvince = p.NameProvince,
                        IdType = p.IdType,                      
                        NameType = p.NameType,
                        Slug = StringHelpers.GenerateSlug(p.Name)
                    }).Where(p => p.Code.Contains(keyword.ToUpper()) || filterString(p.Name.ToLower().Trim()).Contains(keyword));
                    if (lstUniversity != null)
                    {
                        return lstUniversity;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ViewUniversity>> GetUniversityByTypeAndProvince(int? IdType, int? IdProvince)
        {
            try
            {
              
                IEnumerable<ViewUniversity> lstUniversity = null;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    var result = await uow.dbScoreRepository.GetUniversityByTypeAndProvince(IdType,IdProvince);
                    var University = await result.ReadAsync<ViewUniversity>();
                    lstUniversity = University.Select(p => new ViewUniversity
                    {
                        IdUniversity = p.IdUniversity,
                        Code = p.Code,
                        Name = p.Name,
                        IdProvince = p.IdProvince,
                        NameProvince = p.NameProvince,
                        IdType = p.IdType,
                        NameType = p.NameType,
                        Slug = StringHelpers.GenerateSlug(p.Name)
                    });
                    if (lstUniversity != null)
                    {
                        return lstUniversity;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Province>> GetProvince()
        {
            try
            {
                IEnumerable<Province> lstProvince = null;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    var result_province = await uow.dbScoreRepository.GetListProvince();                   
                    var province = await result_province.ReadAsync<Province>();                  
                    lstProvince = province.Select(p => new Province
                    {
                        IdProvince = p.IdProvince,
                        NameProvince = p.NameProvince,
                        OrderBy = p.OrderBy
                    }).OrderByDescending(x => x.OrderBy).ThenBy(x => x.NameProvince);
                    if (lstProvince != null)
                    {
                        return lstProvince;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<University_Type>> GetUniversityType()
        {
            try
            {
                IEnumerable<University_Type> lstUniversityType = null;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    var result_type_university = await uow.dbScoreRepository.GetListTypeUniversity();
                    var type_university = await result_type_university.ReadAsync<University_Type>();
                    lstUniversityType = type_university;
                    if (lstUniversityType != null)
                    {
                        return lstUniversityType;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ViewUniversity_Score>> GetScoreByUniversity(string CodeUniversity,string Year)
        {
            try
            {
                IEnumerable<ViewUniversity_Score> lstScore = null;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    var result = await uow.dbScoreRepository.GetScoreByUniversity(CodeUniversity,Year);
                    var score_university = await result.ReadAsync<ViewUniversity_Score>();
                    lstScore = score_university.Select(p => new ViewUniversity_Score
                    {
                        IdScore = p.IdScore,
                        CodeMajor = p.CodeMajor.Replace("\u0000", String.Empty).Trim(),
                        NameMajor = p.NameMajor.Replace("\u0000", String.Empty).Trim(),
                        CodeCombination = p.CodeCombination.Replace("\u0000", String.Empty).Trim(),
                        Score = p.Score.Replace("\u0000", String.Empty).Trim(),
                        Description = p.Description.Replace("\u0000", String.Empty).Trim(),
                        CodeUniversity = p.CodeUniversity.Replace("\u0000", String.Empty).Trim(),
                        Year = p.Year
                    });
                    if (lstScore != null)
                    {
                        return lstScore;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ViewUniversity_Score>> GetScoreByMajor(string CodeMajor)
        {
            try
            {
                IEnumerable<ViewUniversity_Score> lstScore = null;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    var result = await uow.dbScoreRepository.GetScoreByMajor(CodeMajor);
                    var score_university = await result.ReadAsync<ViewUniversity_Score>();
                    lstScore = score_university.Select(p => new ViewUniversity_Score
                    {
                        IdScore = p.IdScore,
                        CodeMajor = p.CodeMajor.Replace("\u0000", String.Empty).Trim(),
                        NameMajor = p.NameMajor.Replace("\u0000", String.Empty).Trim(),
                        CodeCombination = p.CodeCombination.Replace("\u0000", String.Empty).Trim(),
                        Score = p.Score.Replace("\u0000", String.Empty).Trim(),
                        Description = p.Description.Replace("\u0000", String.Empty).Trim(),
                        CodeUniversity = p.CodeUniversity.Replace("\u0000", String.Empty).Trim(),      
                        Name = p.Name.Replace("\u0000", String.Empty).Trim(),
                        Year = p.Year
                    });
                    if (lstScore != null)
                    {
                        return lstScore;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ViewMajor_Combination>> GetMajorCombination(string CodeMajor)
        {
            try
            {
                IEnumerable<ViewMajor_Combination> lstMajorCombination = null;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    var result = await uow.dbScoreRepository.GetListMajor_Combination(CodeMajor);
                    var major_combi = await result.ReadAsync<ViewMajor_Combination>();
                    lstMajorCombination = major_combi.Select(p => new ViewMajor_Combination
                    {
                        IdMajor_Combination = p.IdMajor_Combination,                       
                        CodeMajor = p.CodeMajor.Replace("\u0000", String.Empty).Trim(),
                        NameMajor = p.NameMajor.Replace("\u0000", String.Empty).Trim(),
                        CodeCombination = p.CodeCombination.Replace("\u0000", String.Empty).Trim(),
                        NameCombination = p.NameCombination.Replace("\u0000", String.Empty).Trim()
                    });
                    if (lstMajorCombination != null)
                    {
                        return lstMajorCombination;
                    }
                }
                return null;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<ViewMajor>> GetMajor()
        {
            try
            {
                IEnumerable<ViewMajor> lstMajor = null;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    var result = await uow.dbScoreRepository.GetListMajor();
                    var major = await result.ReadAsync<ViewMajor>();
                    lstMajor = major.Select(p => new ViewMajor
                    {
                        IdMajor = p.IdMajor,
                        CodeMajor = p.CodeMajor.Replace("\u0000", String.Empty).Trim(),
                        NameMajor = p.NameMajor.Replace("\u0000", String.Empty).Trim(),
                        Hot = p.Hot,
                        Slug = StringHelpers.GenerateSlug(p.NameMajor),
                        IdGroupMajor = p.IdGroupMajor
                    }); ;;
                    if (lstMajor != null)
                    {
                        return lstMajor;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ViewMajor>> GetMajorByQuantity(int quantity)
        {
            try
            {
                IEnumerable<ViewMajor> lstMajor = null;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    var result = await uow.dbScoreRepository.GetListMajorByQuantity(quantity);
                    var major = await result.ReadAsync<ViewMajor>();
                    lstMajor = major.Select(p => new ViewMajor
                    {
                        IdMajor = p.IdMajor,
                        CodeMajor = p.CodeMajor,
                        NameMajor = p.NameMajor,
                        Hot = p.Hot,
                        Slug = StringHelpers.GenerateSlug(p.NameMajor),
                        IdGroupMajor = p.IdGroupMajor,                
                    });
                    if (lstMajor != null)
                    {
                        return lstMajor;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ViewUniversity_Score_and_University>> GetTopMajorByYear(string year, int quantity)
        {
            try
            {
                IEnumerable<ViewUniversity_Score_and_University> lstMajorHaveMaxScore = null;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    var result = await uow.dbScoreRepository.GetTopMajorByYear(year,quantity);
                    var majorHaveMaxScore = await result.ReadAsync<ViewUniversity_Score_and_University>();
                    lstMajorHaveMaxScore = majorHaveMaxScore.Select(p => new ViewUniversity_Score_and_University
                    {
                        IdScore=p.IdScore,
                        CodeMajor=p.CodeMajor.Replace("\u0000",String.Empty).Trim(),
                        NameMajor=p.NameMajor,
                        CodeCombination=p.CodeCombination.Replace("\u0000", String.Empty).Trim(),
                        Score=p.Score,
                        Description=p.Description,
                        Hot=p.Hot,
                        Year=p.Year,
                        CodeUniversity=p.CodeUniversity,
                        Slug = StringHelpers.GenerateSlug(p.NameMajor),
                        Name =p.Name,
                        Type=p.Type,
                        NameType=p.NameType,
                        Province=p.Province,
                        NameProvince=p.NameProvince,
                        Address=p.Address,
                    });
                    if (lstMajorHaveMaxScore != null)
                    {
                        return lstMajorHaveMaxScore;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ViewCombination>> GetCombination()
        {
            try
            {
                IEnumerable<ViewCombination> lstCombination = null;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {

                    var result = await uow.dbScoreRepository.GetListCombination();
                    var combinations = await result.ReadAsync<ViewCombination>();
                    lstCombination = combinations.Select(p => new ViewCombination
                    {                    
                        IdCombination = p.IdCombination,
                        CodeCombination = p.CodeCombination.Replace("\u0000", String.Empty).Trim(),
                        NameCombination = p.NameCombination.Replace("\u0000", String.Empty).Trim(),
                    });
                    if (lstCombination != null)
                    {
                        return lstCombination;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ViewMajorGroup>> GetGroupMajor()
        {
            try
            {
                List<ViewMajorGroup> lstGroupMajor = new List<ViewMajorGroup>();
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    var result = await uow.dbScoreRepository.GetGroupMajor();
                    var groupMajor = await result.ReadAsync<ViewMajorGroup>();
                    foreach (var item in groupMajor)
                    {
                        ViewMajorGroup GroupMajor = new ViewMajorGroup();
                        GroupMajor.IdGroupMajor = item.IdGroupMajor;
                        GroupMajor.NameGroupMajor = item.NameGroupMajor;
                        lstGroupMajor.Add(GroupMajor);
                    }
                    if (lstGroupMajor != null)
                    {
                        return lstGroupMajor;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ViewMajorGroup>> GetGroupMajor_Major()
        {
            try
            {
                List<ViewMajorGroup> lstGroupMajor = new List<ViewMajorGroup>();
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {                 
                    var result = await uow.dbScoreRepository.GetGroupMajor();
                    var groupMajor = await result.ReadAsync<ViewMajorGroup>();
                    foreach (var item in groupMajor)
                    {                     
                        var result_major = await uow.dbScoreRepository.GetListGroupMajor(item.IdGroupMajor);
                        var major = await result_major.ReadAsync<MajorItem>();
                        ViewMajorGroup GroupMajor = new ViewMajorGroup();
                        GroupMajor.IdGroupMajor = item.IdGroupMajor;
                        GroupMajor.NameGroupMajor = item.NameGroupMajor;
                        GroupMajor.Major = major;
                        lstGroupMajor.Add(GroupMajor);
                    }
                    if (lstGroupMajor != null)
                    {
                        return lstGroupMajor;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<ViewPredictScore>> PredictScoreByCodeMajor(float FromPoint, float ToPoint, string CodeMajor, string CodeCombination, int? Province)
        {
            try
            {
               
                IEnumerable<ViewPredictScore> lstUniversity = null;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    var result = await uow.dbScoreRepository.PredictScoreByCodeMajor(FromPoint,ToPoint,CodeMajor, CodeCombination, Province);
                    var uiversity = await result.ReadAsync<ViewPredictScore>();
                    lstUniversity = uiversity.Select(p => new ViewPredictScore
                    {
                        Code = p.Code.Replace("\u0000", String.Empty).Trim(),
                        Name = p.Name.Replace("\u0000", String.Empty).Trim(),
                        Province = (int)Province,
                        FromPoint = FromPoint,
                        ToPoint = ToPoint,
                        CodeMajor = CodeMajor.Replace("\u0000", String.Empty).Trim(),
                        CodeCombination = CodeCombination
                    });
                    if (lstUniversity != null)
                    {
                        return lstUniversity;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<DetailPredictScore>> DetailPredictScoreByCodeMajor(float FromPoint, float ToPoint, string CodeUniversity, string CodeMajor, string CodeCombination)
        {
            try
            {
              
                IEnumerable<DetailPredictScore> lstDetail= null;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    var result = await uow.dbScoreRepository.DetailPredictScoreByCodeMajor(FromPoint, ToPoint, CodeUniversity, CodeMajor,CodeCombination);
                    var uiversity = await result.ReadAsync<DetailPredictScore>();
                    lstDetail = uiversity.Select(p => new DetailPredictScore
                    {
                        NameMajor = p.NameMajor.Replace("\u0000", String.Empty).Trim(),
                        CodeMajor = p.CodeMajor.Replace("\u0000", String.Empty).Trim(),
                        CodeCombination = p.CodeCombination.Replace("\u0000", String.Empty).Trim(),
                        Score = p.Score,
                        Description = p.Description.Replace("\u0000", String.Empty).Trim(),
                        Year = p.Year
                    }).Where(x => filterString(x.CodeCombination.ToLower().Trim()).Contains(filterString(CodeCombination)));
                    if (lstDetail != null)
                    {
                        return lstDetail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ViewPredictScore>> PredictScoreByGroupMajor(float FromPoint, float ToPoint, int GroupMajor, string CodeCombination, int? Province)
        {
            try
            {
               
                IEnumerable<ViewPredictScore> lstUniversity = null;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    var result = await uow.dbScoreRepository.PredictScoreByGroupMajor(FromPoint,ToPoint,GroupMajor,CodeCombination,Province);
                    var uiversity = await result.ReadAsync<ViewPredictScore>();
                    lstUniversity = uiversity.Select(p => new ViewPredictScore
                    {
                        Code = p.Code.Replace("\u0000", String.Empty).Trim(),
                        Name = p.Name.Replace("\u0000", String.Empty).Trim(),
                        Province = p.Province,
                        FromPoint = FromPoint,
                        ToPoint = ToPoint,
                        GroupMajor = GroupMajor,
                        CodeCombination = CodeCombination
                    });
                    if (lstUniversity != null)
                    {
                        return lstUniversity;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<DetailPredictScore>> DetailPredictScoreByGroupMajor(float FromPoint, float ToPoint,string CodeUniversity, int GroupMajor ,string CodeCombination)
        {
            try
            {

                IEnumerable<DetailPredictScore> lstDetail = null;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    var result = await uow.dbScoreRepository.DetailPredictScoreByGroupMajor(FromPoint, ToPoint, CodeUniversity, GroupMajor,CodeCombination);
                    var uiversity = await result.ReadAsync<DetailPredictScore>();
                    lstDetail = uiversity.Select(p => new DetailPredictScore
                    {
                        NameMajor = p.NameMajor.Replace("\u0000", String.Empty).Trim(),
                        CodeMajor = p.CodeMajor.Replace("\u0000", String.Empty).Trim(),
                        CodeCombination = p.CodeCombination.Replace("\u0000", String.Empty).Trim(),
                        Score = p.Score,
                        Description = p.Description.Replace("\u0000", String.Empty).Trim(),
                        Year = p.Year
                    }).Where(x => filterString(x.CodeCombination.ToLower().Trim()).Contains(filterString(CodeCombination)));
                    if (lstDetail != null)
                    {
                        return lstDetail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public async Task<int> CreateUniversity(string Code, string Name, int Type, int Province, string Address, string CreatedBy)
        {
            try
            {
                int result;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    result = await uow.dbScoreRepository.CreateUniversity(Code,Name,Type,Province,Address,CreatedBy);
                    uow.Commit();
                    if (result != 0)
                    {
                        return result;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> UpdateUniversity(int Id, string Code, string Name, int Type, int Province, string Address, string UpdatedBy)
        {
            try
            {
                int result;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    result = await uow.dbScoreRepository.UpdateUniversity(Id,Code, Name, Type, Province,Address, UpdatedBy);
                    uow.Commit();
                    if (result != 0)
                    {
                        return result;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> DeleteUniversity(int Id, string DeletedBy)
        {
            try
            {
                int result;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    result = await uow.dbScoreRepository.DeleteUniversity(Id,DeletedBy);
                    uow.Commit();
                    if (result != 0)
                    {
                        return result;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CreateGroupMajor(string NameGroupMajor, string CreatedBy)
        {
            try
            {
                int result;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    result = await uow.dbScoreRepository.CreateGroupMajor(NameGroupMajor, CreatedBy);
                    uow.Commit();
                    if (result != 0)
                    {
                        return result;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateGroupMajor(int Id, string NameGroupMajor, string UpdatedBy)
        {
            try
            {
                int result;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    result = await uow.dbScoreRepository.UpdateGroupMajor(Id,NameGroupMajor,UpdatedBy);
                    uow.Commit();
                    if (result != 0)
                    {
                        return result;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteGroupMajor(int Id, string DeletedBy)
        {
            try
            {
                int result;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    result = await uow.dbScoreRepository.DeleteGroupMajor(Id, DeletedBy);
                    uow.Commit();
                    if (result != 0)
                    {
                        return result;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CreateMajor(string CodeMajor, string NameMajor, bool Hot, string CreatedBy,int IdGroupMajor)
        {
            try
            {
                int result;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    result = await uow.dbScoreRepository.CreateMajor(CodeMajor,NameMajor,Hot, CreatedBy,IdGroupMajor);
                    uow.Commit();
                    if (result != 0)
                    {
                        return result;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateMajor(int Id, string CodeMajor, string NameMajor,bool Hot, string UpdatedBy,int IdGroupMajor)
        {
            try
            {
                int result;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    result = await uow.dbScoreRepository.UpdateMajor(Id, CodeMajor, NameMajor,Hot, UpdatedBy, IdGroupMajor);
                    uow.Commit();
                    if (result != 0)
                    {
                        return result;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteMajor(int Id, string DeletedBy)
        {
            try
            {
                int result;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    result = await uow.dbScoreRepository.DeleteMajor(Id, DeletedBy);
                    uow.Commit();
                    if (result != 0)
                    {
                        return result;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CreateCombination(string CodeCombination, string NameCombination, string CreatedBy)
        {
            try
            {
                int result;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    result = await uow.dbScoreRepository.CreateCombination(CodeCombination, NameCombination, CreatedBy);
                    uow.Commit();
                    if (result != 0)
                    {
                        return result;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateCombination(int Id, string CodeCombination, string NameCombination, string UpdatedBy)
        {
            try
            {
                int result;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    result = await uow.dbScoreRepository.UpdateCombination(Id, CodeCombination, NameCombination, UpdatedBy);
                    uow.Commit();
                    if (result != 0)
                    {
                        return result;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteCombination(int Id, string DeletedBy)
        {
            try
            {
                int result;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    result = await uow.dbScoreRepository.DeleteCombination(Id, DeletedBy);
                    uow.Commit();
                    if (result != 0)
                    {
                        return result;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CreateScore(string CodeMajor, string NameMajor, string CodeCombination, double Score, string Description, string Year, string CodeUniversity, string CreatedBy)
        {
            try
            {
                int result;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    result = await uow.dbScoreRepository.CreateScore(CodeMajor, NameMajor, CodeCombination, Score, Description, Year, CodeUniversity, CreatedBy);
                    uow.Commit();
                    if (result != 0)
                    {
                        return result;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateScore(int Id, string CodeMajor, string NameMajor, string CodeCombination, double Score, string Description, string Year, string CodeUniversity, string UpdatedBy)
        {
            try
            {
                int result;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    result = await uow.dbScoreRepository.UpdateScore(Id, CodeMajor,NameMajor, CodeCombination,Score,Description,Year,CodeUniversity, UpdatedBy);
                    uow.Commit();
                    if (result != 0)
                    {
                        return result;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteScore(int Id, string DeletedBy)
        {
            try
            {
                int result;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    result = await uow.dbScoreRepository.DeleteScore(Id, DeletedBy);
                    uow.Commit();
                    if (result != 0)
                    {
                        return result;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CreateMajor_Combination(string CodeMajor, string CodeCombination, string CreatedBy)
        {
            try
            {
                int result;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    result = await uow.dbScoreRepository.CreateMajor_Combination(CodeMajor, CodeCombination, CreatedBy);
                    uow.Commit();
                    if (result != 0)
                    {
                        return result;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateMajor_Combination(int Id, string CodeMajor, string CodeCombination, string UpdatedBy)
        {
            try
            {
                int result;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    result = await uow.dbScoreRepository.UpdateMajor_Combination(Id, CodeMajor,CodeCombination,UpdatedBy);
                    uow.Commit();
                    if (result != 0)
                    {
                        return result;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteMajor_Combination(int Id, string DeletedBy)
        {
            try
            {
                int result;
                using (UnitOfWork uow = new UnitOfWork(_connectString))
                {
                    result = await uow.dbScoreRepository.DeleteMajor_Combination(Id, DeletedBy);
                    uow.Commit();
                    if (result != 0)
                    {
                        return result;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
