using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Atiran.DataLayer.Model;
using Atiran.DataLayer.Properties;
using Atiran.DataLayer.Services;

namespace Atiran.DataLayer.Context
{
    public static class Connection
    {
        #region Menu Bar

        private static VersionName ProgramVersion;
        public static List<Menu> ResultAllMenu;

        public static List<SubSystem> ResultAllSubSystem;

        //Alireza 30/07
        private static List<int> HideMenu;

        //-----
        public enum VersionName
        {
            ForshgahiNoskheKamelTakLine = 1,
            OmdeForoshSonatiTakLine = 2,
            MooyragiTakLine = 3,
            MooyragiChandLine = 4,
            ForshgahiNoskheKamelChandLine = 5,
            OmdeForoshSonatiChandLine = 6
        }

        public static void SetMenu() // Alireza 30/07
        {
            GetVersion();
            GetHideMenu();
            using (var ctx = new DBEntities())
            {
                ResultAllMenu = ctx.Menus.Where(m =>
                        (ProgramVersion == VersionName.ForshgahiNoskheKamelTakLine ? m.Form.v1 == true :
                            ProgramVersion == VersionName.OmdeForoshSonatiTakLine ? m.Form.v2 == true :
                            ProgramVersion == VersionName.MooyragiTakLine ? m.Form.v3 == true :
                            ProgramVersion == VersionName.MooyragiChandLine ? m.Form.v4 == true :
                            ProgramVersion == VersionName.ForshgahiNoskheKamelChandLine ? m.Form.v5 == true :
                            ProgramVersion == VersionName.OmdeForoshSonatiChandLine ? m.Form.v6 == true : 1 == 1) &&
                        m.SubSystemID != null || m.FormID == null && ctx.Menus.Any(m1 => m1.ParentMenuID == m.MenuID))
                    .ToList();

                ResultAllSubSystem = ctx.SubSystems.Where(m => !HideMenu.Contains(m.SubSystemId)).ToList();
            }
        }

        private static void GetVersion()
        {
            using (var ctx = new DBEntities())
            {
                var vCode = ctx.overal_setting.Where(o => o.id == 64).Select(o => o.value).FirstOrDefault();
                ProgramVersion = (VersionName) vCode;
            }
        }

        //Alireza 30/07
        private static void GetHideMenu()
        {
            using (var ctx = new DBEntities())
            {
                HideMenu = ctx.overal_setting
                    .Where(o => new List<int> {74, 75, 76, 88, 126}.Contains(o.id) && o.value == 0).Select(o =>
                        o.id == 74 ? 5 : o.id == 75 ? 6 : o.id == 76 ? 10 : o.id == 88 ? 9 : 14).ToList();
            }

            HideMenu.Add(13); // حذف ميانبر از منو
        }
        //-----

        public static List<UserShortcut> GetUserShortcuts(int UserID)
        {
            var Shortcats = new List<UserShortcut>();
            using (var ctx = new DBEntities())
            {
                return ctx.UserFavorites.AsNoTracking().Where(u => u.UserID == UserID).ToList().Select(r =>
                    new UserShortcut
                    {
                        UserID = r.UserID,
                        FormID = r.Menu.Form.FormId,
                        Shortcut = r.Menu.Shortcut,
                        Class = r.Menu.Form.Class,
                        Description = r.Menu.Form.Description,
                        MenuID = r.MenuID,
                        NameSpace = r.Menu.Form.NameSpace,
                        RowID = r.RowID,
                        Text = r.Menu.Text,
                        Ico = Resources.ico,
                        Title = r.Menu.Form.Title,
                        v1 = r.Menu.Form.v1,
                        v2 = r.Menu.Form.v2,
                        v3 = r.Menu.Form.v3,
                        v4 = r.Menu.Form.v4,
                        v5 = r.Menu.Form.v5,
                        v6 = r.Menu.Form.v6,
                        v7 = r.Menu.Form.v7,
                        v8 = r.Menu.Form.v8,
                        v9 = r.Menu.Form.v9,
                        v10 = r.Menu.Form.v10
                    }).ToList();
            }
        }

        public static List<ActiveUser> GetActiveUsers()
        {
            using (var ctx = new DBEntities())
            {
                return ctx.sys_users.AsNoTracking().Where(u => u.IsLoggedIn == true && u.active == true).Select(r =>
                    new ActiveUser
                    {
                        user_id = r.user_id,
                        user_name = r.user_name,
                        user_lname = r.user_lname,
                        user_fname = r.user_fname,
                        user_pic = r.user_pic,
                        IsLoggedIn = r.IsLoggedIn,
                        active = r.active
                    }).ToList();
            }
        }

        public static string GetNameSalMali(int SalMaliID)
        {
            using (var ctx = new DBEntities())
            {
                return ctx.sal_mali.AsNoTracking().FirstOrDefault(s => s.sal_maliID == SalMaliID).name;
            }
        }

        #endregion

        #region Report Sarfasl

        #region private

        private static List<KeyValuePair<int, string>> KindName = new List<KeyValuePair<int, string>>
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
            if (where != null) result = result.Where(where);

            var Row = 1;
            return result.ToList().Select(g => new GroupSarfaslServes
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
            if (where != null) result = result.Where(where);

            var Row = 1;
            return result.ToList().Select(s => new SarfaslService
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
            if (where != null) result = result.Where(where);

            var Row = 1;
            return result.ToList().Select(z => new ZirSarfaslService
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
            if (where != null) result = result.Where(where);

            //var x = (from read in context.act_zirsarfasls
            //    select new
            //    {
            //        ALi=read.zirsarfasls.sarfasls.rdf,
            //    }).ToList();
            var Row = 2;
            return result.ToList().OrderBy(r => r.date).Select(a => new SZAservice
            {
                Arow = Row++,
                AID = a.rdf,
                AZirSarfaslID = a.rdf_zirsarfasls,
                Adate = a.date,
                Abed = a.bed,
                Abes = a.bes,
                Abed_bes = a.bed - a.bes > 0 ? "بد" : a.bed - a.bes == 0 ? "--" : "بس",
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

        public static List<SZAservice> GetSZAServices(string FromDate, string ToDate, List<int> listZirsarfaslID,
            List<int> ListSarfaslID, List<int> ListGroupSarfaslID)
        {
            using (var context = new DBEntities())
            {
                var listG = "";
                var listS = "";
                var listZ = "";

                foreach (var g in ListGroupSarfaslID) listG += g + ",";
                foreach (var s in ListSarfaslID) listS += s + ",";
                foreach (var z in listZirsarfaslID) listZ += z + ",";
                var Group = GetGroupSarfaslses();
                var result = context.USP_GetDataForSarfasl(FromDate, ToDate, listG, listS, listZ);
                var rowZ = 1;
                return result.Select(r => new SZAservice
                {
                    Zrow = rowZ++,
                    ZID = r.ZID,
                    ZName = r.ZName,
                    ZSarfaslID = r.ZSarfaslID,
                    Zbed = r.Zbed ?? 0,
                    Zbes = r.Zbes ?? 0,
                    ZMan = r.ZMan ?? 0,
                    Zbed_bes = r.ZMan > 0 ? "بد" : r.ZMan == 0 ? "--" : "بس",
                    ZMan_Befor = (r.ZMan_All ?? 0) - (r.ZMan ?? 0),
                    Zbed_bes_Befor = (r.ZMan_All ?? 0) - (r.ZMan ?? 0) > 0 ? "بد" :
                        (r.ZMan_All ?? 0) - (r.ZMan ?? 0) == 0 ? "--" : "بس",
                    ZMan_All = r.ZMan_All ?? 0,
                    Zbed_bes_All = (r.ZMan_All ?? 0) > 0 ? "بد" : (r.ZMan_All ?? 0) == 0 ? "--" : "بس",
                    Zhas_dar = r.Zhas_dar.ToLower() == "m" ? "ماليات" :
                        r.Zhas_dar.ToLower() == "h" ? "هزينه" :
                        r.Zhas_dar.ToLower() == "d" ? "دارايي" :
                        r.Zhas_dar.ToLower() == "b" ? "بدون ماليات" : "",
                    ZActive = r.ZActive,
                    SID = r.SID,
                    SGroupSarfaslName = Group.First(s => s.ID == r.SGroupSarfaslID).Name,
                    SGroupSarfaslID = r.SGroupSarfaslID,
                    SName = r.SName,
                    Sbed = r.Sbed ?? 0,
                    Sbes = r.Sbes ?? 0,
                    SMan = r.SMan ?? 0,
                    Sbed_bes = r.SMan > 0 ? "بد" : r.SMan == 0 ? "--" : "بس",
                    SMan_Befor = (r.SMan_All ?? 0) - (r.SMan ?? 0),
                    Sbed_bes_Befor = (r.SMan_All ?? 0 - (r.SMan ?? 0)) > 0 ? "بد" :
                        (r.SMan_All ?? 0) - (r.SMan ?? 0) == 0 ? "--" : "بس",
                    SMan_All = r.SMan_All ?? 0,
                    Sbed_bes_All = (r.SMan_All ?? 0) > 0 ? "بد" : (r.SMan_All ?? 0) == 0 ? "--" : "بس",
                    Shas_dar = r.Shas_dar.ToLower() == "m" ? "ماليات" :
                        r.Shas_dar.ToLower() == "h" ? "هزينه" :
                        r.Shas_dar.ToLower() == "d" ? "دارايي" :
                        r.Shas_dar.ToLower() == "b" ? "بدون ماليات" : "",
                    Swho_def = r.Swho_def
                }).ToList();
            }
        }
        
        public static List<SZAservice> GetActZirSarfaslServices(string FromDate, string ToDate, int sarfaslID,
            List<int> listZirsarfasl)
        {
            var listZ = "";
            foreach (var z in listZirsarfasl) listZ += z + ",";
            using (var context = new DBEntities())
            {
                var result = context.USP_GetDataStimulSoft_Sarfasl_ZirSarfasl(FromDate, ToDate, sarfaslID, listZ)
                    .OrderBy(r => r.date).OrderBy(r => r.rdfzirsarfasl).ToList();
                var Row = 0;
                var backZirsarfaslID = 0;
                var aid = 0;
                var Man_Befor = result.GroupBy(g => new
                        {g.bed_gh, g.bes_gh, g.NameZirsarfasl, g.rdfzirsarfasl, g.NameSarfasl, g.rdfsarfasl})
                    .Select(r => new SZAservice
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
                var result1 = result.Select(r => new SZAservice
                {
                    Arow = backZirsarfaslID == r.rdfzirsarfasl ? ++Row : Row = 1,
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
        }

        #endregion

        #endregion
    }
}