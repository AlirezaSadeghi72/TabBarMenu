using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Atiran.DataLayer.Model;
using Atiran.DataLayer.Services;

namespace Atiran.DataLayer.Context
{
    public static class Connection
    {
        #region Menu Bar
        private static VersionName ProgramVersion;
        public static List<Menu> ResultAllMenu;
        public static List<SubSystem> ResultAllSubSystem;

        public enum VersionName
        {
            ForshgahiNoskheKamelTakLine = 1,
            OmdeForoshSonatiTakLine = 2,
            MooyragiTakLine = 3,
            MooyragiChandLine = 4,
            ForshgahiNoskheKamelChandLine = 5,
            OmdeForoshSonatiChandLine = 6,
        }

        public static void SetMenu()
        {
            using (var ctx = new DBEntities())
            {
                ResultAllMenu = ctx.Menus.Where(m => ((ProgramVersion == Connection.VersionName.ForshgahiNoskheKamelTakLine ? m.Form.v1 == true : ProgramVersion == Connection.VersionName.OmdeForoshSonatiTakLine ? m.Form.v2 == true : ProgramVersion == Connection.VersionName.MooyragiTakLine ? m.Form.v3 == true : ProgramVersion == Connection.VersionName.MooyragiChandLine ? m.Form.v4 == true : ProgramVersion == Connection.VersionName.ForshgahiNoskheKamelChandLine ? m.Form.v5 == true : ProgramVersion == Connection.VersionName.OmdeForoshSonatiChandLine ? m.Form.v6 == true : 1 == 1) || m.FormID == null ) && m.SubSystemID != null &&(ctx.Menus.Any(m1=>m1.ParentMenuID == m.MenuID) || m.FormID != null)).ToList();
                ResultAllSubSystem = ctx.SubSystems.ToList();
            }
        }
        public static void GetVersion()
        {
            using (var ctx = new DBEntities())
            {
                long vCode = ctx.overal_setting.Where(o => o.id == 64).Select(o => o.value).FirstOrDefault();
                ProgramVersion = (VersionName)vCode;
            }
        }

        #endregion

        #region Report Sarfasl

        #region private

        private static List<KeyValuePair<int, string>> KindName = new List<KeyValuePair<int, string>>()
        {
            new KeyValuePair<int, string>(51, "از صندوق به سرفصل"),
            new KeyValuePair<int, string>(52, "از اسناد پرداختي و صندوق-نقد و چك به سرفصل"),
            new KeyValuePair<int, string>(53, "از اسناد پرداختي-چك به سرفصل"),
            new KeyValuePair<int, string>(54, "از سرفصل به سرفصل"),
            new KeyValuePair<int, string>(55, "از سرفصل به حساب جاري"),
            new KeyValuePair<int, string>(56, "از سرفصل به مشتري"),
            new KeyValuePair<int, string>(57, "از سرفصل به صندوق"),
            new KeyValuePair<int, string>(58, "از جاري به سرفصل"),
            new KeyValuePair<int, string>(59, "از جاري به جاري"),
            new KeyValuePair<int, string>(60, "از جاري به مشتري"),
            new KeyValuePair<int, string>(61, "از جاري به صندوق"),
            new KeyValuePair<int, string>(62, "از اسناد دريافتي(خرج چك) به سرفصل"),
            new KeyValuePair<int, string>(63, "از صندوق و اسناد دريافتي به سرفصل"),
            new KeyValuePair<int, string>(64, "از مشتري به سرفصل"),
            new KeyValuePair<int, string>(65, "از انبار به سرفصل")
        };
        private static List<GroupSarfaslServes> _getGroupSarfaslses(DBEntities context,
            Expression<Func<GroupSarfasl, bool>> where = null)
        {
            IQueryable<GroupSarfasl> result = context.Set<GroupSarfasl>().AsNoTracking();
            if (where != null)
            {
                result = result.Where(where);
            }

            int Row = 1;
            return result.ToList().Select(g => new GroupSarfaslServes()
            {
                row = Row++,
                ID = g.GroupSarfaslID,
                Name = g.GroupSarfaslName,
                Active = g.Active
            }).ToList();

        }
        private static List<SarfaslService> _getSarfaslses(DBEntities context,
            Expression<Func<sarfasl, bool>> where = null)
        {
            IQueryable<sarfasl> result = context.Set<sarfasl>().AsNoTracking();
            if (where != null)
            {
                result = result.Where(where);
            }

            int Row = 1;
            return result.ToList().Select(s => new SarfaslService()
            {
                row = Row++,
                ID = s.rdf,
                GroupSarfaslID = s.GroupSarfaslID,
                Name = s.name
            }).ToList();

        }

        private static List<ZirSarfaslService> _getZirSarfasls(DBEntities context,
            Expression<Func<zirsarfasl, bool>> where = null)
        {
            IQueryable<zirsarfasl> result = context.Set<zirsarfasl>().AsNoTracking();
            if (where != null)
            {
                result = result.Where(where);
            }

            int Row = 1;
            return result.ToList().Select(z => new ZirSarfaslService()
            {
                row = Row++,
                ID = z.rdf,
                SarfaslID = z.rdf_sarfasl,
                Name = z.name,
                has_dar = z.has_dar,
                Active = z.Active
            }).ToList();
        }

        private static List<SZAservice> _getActZirSarfasls(DBEntities context,
            Expression<Func<act_zirsarfasls, bool>> where = null)
        {
            IQueryable<act_zirsarfasls> result = context.Set<act_zirsarfasls>().AsNoTracking();
            if (where != null)
            {
                result = result.Where(where);
            }

            //var x = (from read in context.act_zirsarfasls
            //    select new
            //    {
            //        ALi=read.zirsarfasls.sarfasls.rdf,
            //    }).ToList();
            int Row = 2;
            return result.ToList().OrderBy(r => r.date).Select(a => new SZAservice()
            {
                Arow = Row++,
                AID = a.rdf,
                AZirSarfaslID = a.rdf_zirsarfasls,
                Adate = a.date,
                Abed = a.bed,
                Abes = a.bes,
                Abed_bes = (a.bed - a.bes) > 0 ? "بد" : (a.bed - a.bes) == 0 ? "--" : "بس",
                Adescription = a.dis,
                Akind = a.kind,
                AkindName = KindName.FirstOrDefault(k => k.Key == a.kind).Value,
                Asanadno = a.sanadno,
                Auser = a.user
            }).ToList();

        }

        #endregion

        #region public
        public static List<GroupSarfaslServes> GetGroupSarfaslses(Expression<Func<GroupSarfasl, bool>> where = null)
        {
            using (var context = new DBEntities())
            {
                return _getGroupSarfaslses(context, where);
            }
        }
        public static List<SarfaslService> GetSarfaslses(Expression<Func<sarfasl, bool>> where = null)
        {
            using (var context = new DBEntities())
            {
                return _getSarfaslses(context, where);
            }
        }

        public static List<ZirSarfaslService> GetZirSarfasls(Expression<Func<zirsarfasl, bool>> where = null)
        {
            using (var context = new DBEntities())
            {
                return _getZirSarfasls(context, where);
            }
        }

        //public static List<SZAservice> GetActZirSarfasls(Expression<Func<act_zirsarfasls, bool>> where = null)
        //{
        //    using (var context = new DBEntities())
        //    {
        //        return _getActZirSarfasls(context, where);
        //    }
        //}

        //public static List<SZAservice> GetSarfaslseServis(List<int> listSarfaslID, List<int> listZirsarfaslID, string FromDate, string ToDate)
        //{//return new List<SarfaslService>();
        //    using (var context = new DBEntities())
        //    {
        //        string listS = "";
        //        foreach (int s in listSarfaslID)
        //        {
        //            listS += s + ",";
        //        }
        //        string listZ = "";
        //        foreach (int z in listZirsarfaslID)
        //        {
        //            listZ += z + ",";
        //        }
        //        //return new List<SarfaslService>();
        //        var result = context.USP_GetSarfaslseServis(listS, listZ, FromDate, ToDate);
        //        int Row = 1;
        //        return result.Select(r => new SZAservice()
        //        {
        //            Srow = Row++,
        //            SID = r.rdf,
        //            SName = r.name,
        //            SGroupSarfaslID = r.GroupSarfaslID,
        //            Sbed = r.Bed ?? 0,
        //            Sbes = r.Bes ?? 0,
        //            SMan = r.Man ?? 0,
        //            Sbed_bes = (r.Man > 0) ? "بد" : (r.Man == 0) ? "--" : "بس",
        //            SMan_Befor = (r.Man_All ?? 0),
        //            Sbed_bes_Befor = ((r.Man_All ?? 0)  > 0) ? "بد" : ((r.Man_All ?? 0) == 0) ? "--" : "بس",
        //            Swho_def = r.who_def,
        //            Shas_dar = (r.has_dar.ToLower() == "m") ? "ماليات" : (r.has_dar.ToLower() == "h") ? "هزينه" : (r.has_dar.ToLower() == "d") ? "دارايي" : (r.has_dar.ToLower() == "b") ? "بدون ماليات" : ""
        //        }).ToList();

        //        #region MyRegion


        //        //////////////////////////////////////////////////////////////////////////////////////////////
        //        //var result = context.act_zirsarfasls.AsNoTracking().Join(context.zirsarfasls.AsNoTracking(),
        //        //    a => a.rdf_zirsarfasls,
        //        //    z => z.rdf,
        //        //    (a, z) => new { a, z }).Join(context.sarfasls.AsNoTracking(), az => az.z.rdf_sarfasl, s => s.rdf,
        //        //    (az, s) => new { az, s });

        //        //if (listZirsarfaslID.Any())
        //        //{
        //        //    result = result.Where(r => listZirsarfaslID.Contains(r.az.z.rdf));
        //        //}

        //        //if (listSarfaslID.Any())
        //        //{
        //        //    result = result.Where(r => listSarfaslID.Contains(r.s.rdf));
        //        //}
        //        //else if (listZirsarfaslID.Any())
        //        //{
        //        //    listSarfaslID = context.zirsarfasls.Where(z => listZirsarfaslID.Contains(z.rdf))
        //        //        .GroupBy(z => z.rdf_sarfasl).Select(z => z.Key).ToList();
        //        //}
        //        //else
        //        //{
        //        //    listSarfaslID = context.sarfasls.Select(s => s.rdf).ToList();
        //        //}

        //        //result = result.Where(r => r.az.a.date.CompareTo(FromDate) >= 0 && r.az.a.date.CompareTo(ToDate) <= 0);

        //        ////var ali = result.GroupBy(r2 => r2.s.rdf).Select(
        //        ////    g => new
        //        ////{
        //        ////    sarfasl = g.Select(r=>r.s),
        //        ////    man = g.Sum(r1 => r1.az.a.bed - r1.az.a.bes)
        //        ////    }).ToList();
        //        //int Row = 1;
        //        //var result1 = result.GroupBy(r2 => new { r2.s }).ToList().Select(
        //        //    g => new SarfaslService()
        //        //    {
        //        //        row = Row++,
        //        //        ID = g.Key.s.rdf,
        //        //        Name = g.Key.s.name,
        //        //        GroupSarfaslID = g.Key.s.GroupSarfaslID,
        //        //        Man = g.Sum(r1 => r1.az.a.bed - r1.az.a.bes)
        //        //    }).ToList();

        //        //if (listSarfaslID.Count > result1.Count)
        //        //{
        //        //    var Sarfasls = context.sarfasls.AsNoTracking().ToList();

        //        //    foreach (int id in listSarfaslID)
        //        //    {
        //        //        if (!result1.Any(r => r.ID == id))
        //        //        {
        //        //            var Safasl = Sarfasls.First(s => s.rdf == id);
        //        //            result1.Add(new SarfaslService()
        //        //            {
        //        //                row = result1.Count + 1,
        //        //                ID = Safasl.rdf,
        //        //                Name = Safasl.name,
        //        //                GroupSarfaslID = Safasl.GroupSarfaslID,
        //        //                Man = 0
        //        //            });
        //        //        }
        //        //    }
        //        //}

        //        //return result1;


        //        #endregion
        //    }

        //    //return (from read in result select new SarfaslService
        //    //{
        //    //    GroupSarfaslID = read.GroupSarfaslID
        //    //}).ToList();
        //}

        public static List<SZAservice> GetSZAServices(string FromDate, string ToDate, List<int> listZirsarfaslID, List<int> ListSarfaslID, List<int> ListGroupSarfaslID)
        {
            using (var context = new DBEntities())
            {
                string listG = "";
                string listS = "";
                string listZ = "";

                foreach (int g in ListGroupSarfaslID)
                {
                    listG += g + ",";
                }
                foreach (int s in ListSarfaslID)
                {
                    listS += s + ",";
                }
                foreach (int z in listZirsarfaslID)
                {
                    listZ += z + ",";
                }
                List<GroupSarfaslServes> Group = GetGroupSarfaslses();
                var result = context.USP_GetDataForSarfasl(FromDate, ToDate, listG, listS, listZ);
                int rowZ = 1;
                return result.Select(r => new SZAservice()
                {
                    Zrow = rowZ++,
                    ZID = r.ZID,
                    ZName = r.ZName,
                    ZSarfaslID = r.ZSarfaslID,
                    Zbed = r.Zbed ?? 0,
                    Zbes = r.Zbes ?? 0,
                    ZMan = r.ZMan ?? 0,
                    Zbed_bes = (r.ZMan > 0) ? "بد" : (r.ZMan == 0) ? "--" : "بس",
                    ZMan_Befor = (r.ZMan_All ?? 0) - (r.ZMan ?? 0),
                    Zbed_bes_Befor = (((r.ZMan_All ?? 0) - (r.ZMan ?? 0)) > 0) ? "بد" : (((r.ZMan_All ?? 0) - (r.ZMan ?? 0)) == 0) ? "--" : "بس",
                    ZMan_All = (r.ZMan_All ?? 0),
                    Zbed_bes_All = ((r.ZMan_All ?? 0) > 0) ? "بد" : ((r.ZMan_All ?? 0) == 0) ? "--" : "بس",
                    Zhas_dar = (r.Zhas_dar.ToLower() == "m") ? "ماليات" : (r.Zhas_dar.ToLower() == "h") ? "هزينه" : (r.Zhas_dar.ToLower() == "d") ? "دارايي" : (r.Zhas_dar.ToLower() == "b") ? "بدون ماليات" : "",
                    ZActive = r.ZActive,
                    SID = r.SID,
                    SGroupSarfaslName = Group.First(s => s.ID == r.SGroupSarfaslID).Name,
                    SGroupSarfaslID = r.SGroupSarfaslID,
                    SName = r.SName,
                    Sbed = r.Sbed ?? 0,
                    Sbes = r.Sbes ?? 0,
                    SMan = r.SMan ?? 0,
                    Sbed_bes = (r.SMan > 0) ? "بد" : (r.SMan == 0) ? "--" : "بس",
                    SMan_Befor = (r.SMan_All ?? 0) - (r.SMan ?? 0),
                    Sbed_bes_Befor = ((r.SMan_All ?? 0 - (r.SMan ?? 0)) > 0) ? "بد" : (((r.SMan_All ?? 0) - (r.SMan ?? 0)) == 0) ? "--" : "بس",
                    SMan_All = (r.SMan_All ?? 0),
                    Sbed_bes_All = ((r.SMan_All ?? 0) > 0) ? "بد" : ((r.SMan_All ?? 0) == 0) ? "--" : "بس",
                    Shas_dar = (r.Shas_dar.ToLower() == "m") ? "ماليات" : (r.Shas_dar.ToLower() == "h") ? "هزينه" : (r.Shas_dar.ToLower() == "d") ? "دارايي" : (r.Shas_dar.ToLower() == "b") ? "بدون ماليات" : "",
                    Swho_def = r.Swho_def
                }).ToList();
            }
        }
        //public static List<SZAservice> GetZirSarfaslServices(List<int> listZirsarfaslID, int sarfaslID, string FromDate, string ToDate)
        //{
        //    using (var context = new DBEntities())
        //    {
        //        string listZ = "";
        //        ObjectResult<USP_GetZirSarfaslServices_Result> result;
        //        foreach (int z in listZirsarfaslID)
        //        {
        //            listZ += z + ",";
        //        }

        //        result = context.USP_GetZirSarfaslServices(listZ, sarfaslID, FromDate, ToDate);

        //        int Row = 1;
        //        return result.Select(r => new SZAservice()
        //        {
        //            Zrow = Row++,
        //            ZID = r.rdf,
        //            ZName = r.name,
        //            ZSarfaslID = r.rdf_sarfasl,
        //            Zbed = r.Bed ?? 0,
        //            Zbes = r.Bes ?? 0,
        //            ZMan = r.Man ?? 0,
        //            Zbed_bes = (r.Man > 0) ? "بد" : (r.Man == 0) ? "--" : "بس",
        //            ZMan_Befor = (r.Man_All ?? 0),
        //            Zbed_bes_Befor = ((r.Man_All ?? 0) > 0) ? "بد" : ((r.Man_All ?? 0) == 0) ? "--" : "بس",
        //            Zhas_dar = (r.has_dar.ToLower() == "m") ? "ماليات" : (r.has_dar.ToLower() == "h") ? "هزينه" : (r.has_dar.ToLower() == "d") ? "دارايي" : (r.has_dar.ToLower() == "b") ? "بدون ماليات" : "",
        //            ZActive = r.Active
        //        }).ToList();

        //        #region MyRegion


        //        //return new List<ZirSarfaslService>();
        //        //var result = context.act_zirsarfasls.AsNoTracking().Join(context.zirsarfasls.AsNoTracking(),
        //        //    a => a.rdf_zirsarfasls,
        //        //    z => z.rdf,
        //        //    (a, z) => new { a, z }).Where(az => az.z.rdf_sarfasl == sarfaslID).Where(az => az.z.rdf_sarfasl == sarfaslID);

        //        //var ZirsarfaslID = context.zirsarfasls.AsNoTracking().Where(z => z.rdf_sarfasl == sarfaslID).Select(z => z.rdf).ToList();

        //        //if (listZirsarfaslID.Any())
        //        //{
        //        //    listZirsarfaslID = listZirsarfaslID.Where(z => ZirsarfaslID.Contains(z)).ToList();
        //        //    if (listZirsarfaslID.Any())
        //        //    {
        //        //        result = result.Where(az => listZirsarfaslID.Contains(az.z.rdf));
        //        //    }
        //        //    else
        //        //    {
        //        //        result = result.Where(az => az.z.rdf == null);
        //        //        listZirsarfaslID = ZirsarfaslID;
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    listZirsarfaslID = ZirsarfaslID;
        //        //}

        //        //result = result.Where(r => r.a.date.CompareTo(FromDate) >= 0 && r.a.date.CompareTo(ToDate) <= 0);

        //        ////var ali = result.GroupBy(r2 => new { r2.z }).ToList();
        //        //int Row = 1;
        //        //var result1 = result.GroupBy(r2 => new { r2.z }).ToList().Select(
        //        //    g => new ZirSarfaslService()
        //        //    {
        //        //        row = Row++,
        //        //        ID = g.Key.z.rdf,
        //        //        Name = g.Key.z.name,
        //        //        SarfaslID = g.Key.z.rdf_sarfasl,
        //        //        Active = g.Key.z.Active,
        //        //        has_dar = g.Key.z.has_dar,
        //        //        Man = g.Sum(r1 => r1.a.bed - r1.a.bes)
        //        //    }).ToList();

        //        //if (listZirsarfaslID.Count > result1.Count)
        //        //{
        //        //    var ZirSarfasls = context.zirsarfasls.AsNoTracking().ToList();

        //        //    foreach (int id in listZirsarfaslID)
        //        //    {
        //        //        if (!result1.Any(r => r.ID == id))
        //        //        {
        //        //            var ZirSafasl = ZirSarfasls.First(z => z.rdf == id);
        //        //            result1.Add(new ZirSarfaslService()
        //        //            {
        //        //                row = result1.Count + 1,
        //        //                ID = ZirSafasl.rdf,
        //        //                Name = ZirSafasl.name,
        //        //                SarfaslID = ZirSafasl.rdf_sarfasl,
        //        //                Active = ZirSafasl.Active,
        //        //                has_dar = ZirSafasl.has_dar,
        //        //                Man = 0
        //        //            });
        //        //        }
        //        //    }
        //        //}

        //        //return result1;


        //        #endregion
        //    }
        //}
        public static List<SZAservice> GetActZirSarfaslServices(string FromDate, string ToDate, int sarfaslID, List<int> listZirsarfasl)
        {
            string listZ = "";
            foreach (int z in listZirsarfasl)
            {
                listZ += z + ",";
            }
            using (var context = new DBEntities())
            {
                var result = context.USP_GetDataStimulSoft_Sarfasl_ZirSarfasl(FromDate, ToDate, sarfaslID, listZ).OrderBy(r => r.date).OrderBy(r => r.rdfzirsarfasl).ToList();
                int Row = 0;
                int backZirsarfaslID = 0;
                int aid = 0;
                List<SZAservice> Man_Befor = result.GroupBy(g => new { g.bed_gh, g.bes_gh, g.NameZirsarfasl, g.rdfzirsarfasl, g.NameSarfasl, g.rdfsarfasl })
                    .Select(r => new SZAservice()
                    {
                        Arow = 0,
                        AID = --aid,
                        Adescription = "حساب قبلي زيرسرفصل " + r.Key.NameZirsarfasl,
                        Abed = r.Key.bed_gh,
                        Abes = r.Key.bes_gh,
                        ZID = r.Key.rdfzirsarfasl,
                        ZName = r.Key.NameZirsarfasl,
                        Zbed = r.Key.bed_gh,
                        Zbes = r.Key.bes_gh,
                        SID = r.Key.rdfsarfasl,
                        SName = r.Key.NameSarfasl
                    }).ToList();
                List<SZAservice> result1 = result.Select(r => new SZAservice()
                {
                    Arow = (backZirsarfaslID == r.rdfzirsarfasl) ? ++Row : Row = 1,
                    AID = r.rdf,
                    Asanadno = r.sanadno,
                    Adate = r.date,
                    Adescription = r.dis,
                    Abed = r.bed,
                    Abes = r.bes,
                    AZirSarfaslID = backZirsarfaslID = r.rdfzirsarfasl,
                    Akind = r.kind,
                    AkindName = KindName.FirstOrDefault(k => k.Key == r.kind).Value,
                    Auser = r.user,
                    ZID = r.rdfzirsarfasl,
                    ZName = r.NameZirsarfasl,
                    Zbed = r.bed_gh,
                    Zbes = r.bes_gh,
                    SID = r.rdfsarfasl,
                    SName = r.NameSarfasl
                }).ToList();
                result1.AddRange(Man_Befor);
                return result1.OrderBy(o => o.Arow).OrderBy(o => o.ZID).ToList();

            }

            //List<SZAservice> result = new List<SZAservice>()
            //{
            //    new SZAservice()
            //    {
            //        Arow = 1,
            //        AID = 0,
            //        Adescription = "مانده از قبل"
            //    }
            //};
            //using (var context = new DBEntities())
            //{
            //    if (zirSarfaslID != -1)
            //    {
            //        result.AddRange(_getActZirSarfasls(context, a => a.rdf_zirsarfasls == zirSarfaslID && a.date.CompareTo(FromDate) >= 0 && a.date.CompareTo(ToDate) <= 0));
            //        var Befor = context.act_zirsarfasls.AsNoTracking()
            //            .Where(a => a.rdf_zirsarfasls == zirSarfaslID && a.date.CompareTo(FromDate) < 0).Select(a => new { a.bed, a.bes }).ToList();

            //        var bed = result.First(r => r.AID == 0).Abed = Befor.Sum(b => b.bed);
            //        var bes = result.First(r => r.AID == 0).Abes = Befor.Sum(b => b.bes);
            //        result.First(r => r.AID == 0).Abed_bes = (bed - bes > 0) ? "بد" : (bed - bes == 0) ? "--" : "بس";
            //    }
            //    else if (sarfaslID != -1)
            //    {
            //        var ZirsarfaslID = context.zirsarfasls.AsNoTracking().Where(z => z.rdf_sarfasl == sarfaslID).Select(z => z.rdf).ToList();

            //        if (listZirsarfasl.Any())
            //        {
            //            var listZirsarfasl1 = listZirsarfasl.Where(z => ZirsarfaslID.Contains(z)).ToList();
            //            if (listZirsarfasl1.Any())
            //            {
            //                result.AddRange(_getActZirSarfasls(context, a => listZirsarfasl1.Contains(a.rdf_zirsarfasls) && a.date.CompareTo(FromDate) >= 0 && a.date.CompareTo(ToDate) <= 0));
            //                var Befor = context.act_zirsarfasls.AsNoTracking()
            //                    .Where(a => listZirsarfasl1.Contains(a.rdf_zirsarfasls) && a.date.CompareTo(FromDate) < 0).Select(a => new { a.bed, a.bes }).ToList();

            //                var bed = result.First(r => r.AID == 0).Abed = Befor.Sum(b => b.bed);
            //                var bes = result.First(r => r.AID == 0).Abes = Befor.Sum(b => b.bes);
            //                result.First(r => r.AID == 0).Abed_bes = (bed - bes > 0) ? "بد" : (bed - bes == 0) ? "--" : "بس";
            //            }

            //            //return _getActZirSarfasls(context, a => listZirsarfasl.Contains(a.rdf_zirsarfasls) && a.date.CompareTo(FromDate) >= 0 && a.date.CompareTo(ToDate) <= 0);
            //        }
            //        else
            //        {
            //            result.AddRange(_getActZirSarfasls(context, a => ZirsarfaslID.Contains(a.rdf_zirsarfasls) && a.date.CompareTo(FromDate) >= 0 && a.date.CompareTo(ToDate) <= 0));
            //            var Befor = context.act_zirsarfasls.AsNoTracking()
            //                .Where(a => ZirsarfaslID.Contains(a.rdf_zirsarfasls) && a.date.CompareTo(FromDate) < 0).Select(a => new { a.bed, a.bes }).ToList();

            //            var bed = result.First(r => r.AID == 0).Abed = Befor.Sum(b => b.bed);
            //            var bes = result.First(r => r.AID == 0).Abes = Befor.Sum(b => b.bes);
            //            result.First(r => r.AID == 0).Abed_bes = (bed - bes > 0) ? "بد" : (bed - bes == 0) ? "--" : "بس";

            //        }
            //    }
            //    return result;

            //}
        }

        //public static List<SZAservice> GetDataForReport(string FromDate, string ToDate, List<int> listZirsarfaslID,
        //    List<int> listZirsarfasl)
        //{
        //    using (var context = new DBEntities())
        //    {
        //        //var resurl = context.SZ_ReportView.Where(v=>)
        //    }

        //    return new List<SZAservice>();
        //}

        //public static decimal manSarfasls(int sarfaslID)
        //{
        //    using (var context = new DBEntities())
        //    {
        //        List<ZirSarfaslService> zs = _getZirSarfasls(context, z => z.rdf_sarfasl == sarfaslID);
        //        List<ActZirSarfaslService> azs = _getActZirSarfasls(context,
        //            a => zs.Select(z => z.ID).ToList().Contains(a.rdf_zirsarfasls));
        //        return azs.Any() ? azs.Sum(a => a.bed - a.bes) : 0;
        //    }
        //}

        //public static decimal manZirSarfasls(int zirSarfaslID)
        //{
        //    using (var context = new DBEntities())
        //    {
        //        List<ActZirSarfaslService> azs = _getActZirSarfasls(context, a => a.rdf_zirsarfasls == zirSarfaslID);
        //        return azs.Any() ? azs.Sum(a => a.bed - a.bes) : 0;
        //    }
        //}

        #endregion

        //public static IEnumerable<T> get<T>(Expression<Func<T, bool>> where = null) where T : class
        //{
        //    using (var context = new DBEntities())
        //    {
        //        IQueryable<T> result = context.Set<T>().AsNoTracking();
        //        if (where != null)
        //        {
        //            result = result.Where(where);
        //        }

        //        return result.ToList();
        //    }
        //}

        #endregion
    }
}