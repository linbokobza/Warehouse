using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WareHouse22.Models;
using WareHouse22.ViewModel;
using static System.Net.Mime.MediaTypeNames;

namespace WareHouse22.Controllers
{
    public class UsersController : Controller
    {
        private UsersDAL db = new UsersDAL();
        private EquipmentDAL Equipmentdb = new EquipmentDAL();
        private LoansDAL Loansdb = new LoansDAL();
        private RoomsDAL Roomsdb = new RoomsDAL();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }
        public ActionResult Home()
        {

            return View("Home");
        }

        public ActionResult UserProfile()
        {
            return View("UserProfile");
        }

        public ActionResult Registration()
        {
            return View("Registration");
        }
        [HttpPost]
        public ActionResult UserRegister(string Email, string FirstName, string LastName, string Password, string Role)
        {
            if (CheckUserExists(Email) == true)
            {
                return Json(new { status = "false" });
            }
            else
            {
                tblUsers u = new tblUsers();
                u.Email = Email;
                u.FirstName = FirstName;
                u.LastName = LastName;
                u.Password = Password;
                u.Role = Role;
                db.Users.Add(u);
                db.SaveChanges();
                return Json(new { status = "true" });
            }
        }
        [HttpPost]
        public ActionResult AddItem(string Code, string Name, string State, string Category, string Instruction, int MaxLoanTime)
        {
            if (CheckItemExists(Code) == false)
            {
                tblEquipment u = new tblEquipment();
                u.Code = Code;
                u.Name = Name;
                u.State = State;
                u.Category = Category;
                u.MaxLoanTime = MaxLoanTime;
                u.Instruction = Instruction;
                Equipmentdb.Equipment.Add(u);
                Equipmentdb.SaveChanges();
                return Json(new { status = "true" });
            }
            else
                return Json(new { status = "false" });
        }

        public Boolean CheckUserExists(string Email)
        {

            foreach (tblUsers u in db.Users)
            {
                if (u.Email == Email)
                {
                    return true;
                }
            }
            return false;
        }

        [HttpPost]
        public ActionResult UserSignIn(string Email, string Password)
        {
            foreach (tblUsers u in db.Users)
            {
                if (u.Email == Email && u.Password == Password)
                {
                    return Json(new { status = "true", name = u.FirstName, role = u.Role });
                }
            }
            return Json(new { status = "false" });
        }
        public Boolean CheckItemExists(string code)
        {
            foreach (tblEquipment u in Equipmentdb.Equipment)
            {
                if (u.Code == code)
                    return true;
            }
            return false;
        }


        [HttpPost]
        public ActionResult ShowEquipment(string SearchInput)
        {
            SearchInput = SearchInput.ToLower();
            EquipmentViewModel f = new EquipmentViewModel();
            List<tblEquipment> equipmentlist = new List<tblEquipment>();
            foreach (tblEquipment u in Equipmentdb.Equipment)
            {
                if (u.Category.ToLower().Equals(SearchInput) || u.Code.ToLower().Contains(SearchInput) || u.Name.ToLower().Contains(SearchInput) || u.State.ToLower().Contains(SearchInput))
                    equipmentlist.Add(u);
            }
            f.ShowEquipments = equipmentlist.ToList();
            return Json(new { equipmentlist });
        }

        [HttpPost]
        public ActionResult LoanItem(string Code, string Email, string StartDate, string ReturnDate)
        {
            int MaxLoanTime = 0;
            if (!CheckLoanAvailableDates(Code, StartDate, ReturnDate))
            {
                return Json(new { status = "false", reason = "taken" });
            }
            if (!CheckLoanAvailableDatesTime(Code, StartDate, ReturnDate, ref MaxLoanTime))
            {
                return Json(new { status = "false", reason = "ToLong", maxloan = MaxLoanTime });
            }
             if (!CheckDatesFormat(StartDate, ReturnDate))
            {
                return Json(new { status = "false", reason = "Format" });
            }
            string name = "";
            foreach (tblEquipment u in Equipmentdb.Equipment)
            {
                if (u.Code == Code)
                    name = u.Name;
            }
            DateTime Start = DateTime.Parse(StartDate);
            DateTime End = DateTime.Parse(ReturnDate);
            tblLoans l = new tblLoans();
            l.ECode = Code;
            l.UserEmail = Email;
            l.ItemName = name;
            l.StartDate = Start;
            l.FinalDate = End;
            l.Returned = "F";
            Loansdb.Loans.Add(l);
            Loansdb.SaveChanges();
            return Json(new { status = "true" });

        }
        
        public Boolean CheckIfAdmin(string mail)
        {
            mail = mail.ToLower();
            foreach(tblUsers u in db.Users)
            {
                if(u.Email.ToLower().Equals(mail) && u.Role.Equals("A"))
                {
                    return true;
                }
            }
            return false;
        }
           
        
         public Boolean CheckDatesFormat(string StartDate,string ReturnDate)
        {
            DateTime Start = DateTime.Parse(StartDate);
            DateTime End = DateTime.Parse(ReturnDate);
            if (End < Start)
            {
                return false;
            }
            return true;
        }
        //check if the item is already taken on those dates
        public Boolean CheckLoanAvailableDates(string Code, string StartDate, string ReturnDate)
        {
            DateTime Start = DateTime.Parse(StartDate);
            if (Start < DateTime.Today.AddDays(-1)) { return false; }
            DateTime End = DateTime.Parse(ReturnDate);
            foreach (tblLoans l in Loansdb.Loans)
            {
                if (l.ECode == Code && ((Start >= l.StartDate && Start <= l.FinalDate) || (End >= l.StartDate && End <= l.FinalDate)))
                    return false;
            }
            return true;
        }

        //check if user want to loan for to much time
        public Boolean CheckLoanAvailableDatesTime(string Code, string StartDate, string ReturnDate, ref int MaxLoanTime)
        {
            DateTime Start = DateTime.Parse(StartDate);
            DateTime End = DateTime.Parse(ReturnDate);
            TimeSpan timespan = End - Start;
            int LoanInDays = timespan.Days;
            foreach (tblEquipment u in Equipmentdb.Equipment)
            {
                if (u.Code == Code)
                {
                    if (LoanInDays > u.MaxLoanTime)
                    {

                        MaxLoanTime = (int)u.MaxLoanTime;
                        return false;
                    }
                }
            }
            return true;
        }

        [HttpPost]
        public ActionResult ShowHistory(string userEmail)
        {
            userEmail = userEmail.ToLower();
            LoansViewModel f = new LoansViewModel();
            List<tblLoans> LoanList = new List<tblLoans>();
            if (CheckIfAdmin(userEmail))
            {
                foreach (tblLoans u in Loansdb.Loans)
                {
                    if (u.FinalDate< DateTime.Today|| u.Returned.Equals("T"))
                        LoanList.Add(u);
                }
            }
            else
            {
                foreach (tblLoans u in Loansdb.Loans)
                {

                    if (u.UserEmail.ToLower().Equals(userEmail) && (u.FinalDate < DateTime.Today || u.Returned.Equals("T")))
                    {

                        LoanList.Add(u);
                    }
                }
            }
            f.ShowLoans = LoanList.ToList();
            return Json(new { LoanList });
        }

        [HttpPost]
        public ActionResult ShowCurrent(string userEmail)
        {
            LoansViewModel f = new LoansViewModel();
            List<tblLoans> LoanList = new List<tblLoans>();
            userEmail = userEmail.ToLower();
            if (CheckIfAdmin(userEmail))
            {
                foreach (tblLoans u in Loansdb.Loans) {
                    if ((u.StartDate >= DateTime.Today || u.FinalDate == DateTime.Today) && u.Returned.Equals("F"))
                        LoanList.Add(u);
                }
            }
            else
            {
                foreach (tblLoans u in Loansdb.Loans)
                {
                    if (u.UserEmail.ToLower().Equals(userEmail) && (u.StartDate >= DateTime.Today|| u.FinalDate==DateTime.Today) && u.Returned.Equals("F"))
                    {
                        LoanList.Add(u);
                    }
                }
            }
            f.ShowLoans = LoanList.ToList();
            return Json(new { LoanList });
        }

        [HttpPost]
        public ActionResult CancelLoan(string userEmail, string code, string startDate, string finalDate)
        {
            try
            {
                DateTime Start = DateTime.Parse(startDate);
                DateTime End = DateTime.Parse(finalDate);
                foreach (tblLoans u in Loansdb.Loans)
                {

                    if (u.UserEmail.Equals(userEmail) && u.StartDate == Start && u.FinalDate == End && u.ECode.Equals(code))
                    {
                        Loansdb.Loans.Remove(u);
                    }
                }
                Loansdb.SaveChanges();
                return Json(new { status = "true" });
            }
            catch
            {
                return Json(new { status = "false" });
            }
        }

        [HttpPost]
        public ActionResult ChangeLoanItem(string Code, string Email, string StartDate, string ReturnDate, string newStartDate, string newReturnDate)
        {
            int MaxLoan=0;
            foreach (tblEquipment u in Equipmentdb.Equipment)
            {
                if (u.Code.Equals(Code))
                    MaxLoan = (int)u.MaxLoanTime;
            }
            CancelLoan( Email,Code, StartDate, ReturnDate);
            if (CheckLoanAvailableDates(Code, newStartDate, newReturnDate) &&
                CheckLoanAvailableDatesTime(Code, newStartDate, newReturnDate, ref MaxLoan)) {
                return LoanItem(Code, Email, newStartDate, newReturnDate);
            }
            else {
                LoanItem(Code, Email, StartDate, ReturnDate);
                return Json(new { status = "false" });
            }
        }

        [HttpPost]
            public ActionResult ShowReports()
            {
                int NumUsers = db.Users.Count(), NumLoans = Loansdb.Loans.Count(), NumCurrentLoans = 0, NumEquipment = Equipmentdb.Equipment.Count(), NumBadEquipment = 0;
                string mail="";
                int count = 0, max = 0;
                foreach(tblLoans u in Loansdb.Loans)
                {
                    if (u.StartDate >= DateTime.Today) { NumCurrentLoans++; }
                    foreach(tblLoans l in Loansdb.Loans)
                    {
                        if(u.UserEmail.Equals(l.UserEmail))
                            count++;
                    }
                    if (count > max) {
                        mail=String.Copy(u.UserEmail);
                        max = count;
                    }
                    count = 0;
                }
                foreach (tblEquipment u in Equipmentdb.Equipment)
                {
                    if (u.State.Equals("Bad")) { NumBadEquipment++; }
                }
                return Json(new { NumU = NumUsers, NumL = NumLoans, NumE = NumEquipment, NumCL = NumCurrentLoans, NumBE = NumBadEquipment, Email=mail });
            }
        [HttpPost]
        public ActionResult EditItem(string Code, string Name, string State, string Category, string Instruction, int MaxLoanTime)
        {

            if (CheckItemExists(Code) == true)
            {
                tblEquipment ed = new tblEquipment();
                foreach (tblEquipment u in Equipmentdb.Equipment)
                {
                    if (u.Code == Code)
                    {
                        ed.Code = Code;
                        if (string.IsNullOrEmpty(Name) == true)
                            ed.Name = u.Name;
                        else ed.Name = Name;
                        if (State.Equals("Select state"))
                            ed.State = u.State;
                        else ed.State = State;
                        if (Category.Equals("Select category"))
                            ed.Category = u.Category;
                        else ed.Category = Category;
                        if (string.IsNullOrEmpty(Instruction) == true)
                            ed.Instruction = u.Instruction;
                        else ed.Instruction = Instruction;
                        if (string.IsNullOrEmpty(MaxLoanTime.ToString()) == true)
                            ed.MaxLoanTime = u.MaxLoanTime;
                        else ed.MaxLoanTime = MaxLoanTime;
                        Equipmentdb.Equipment.Remove(u);
                    }
                }

                Equipmentdb.Equipment.Add(ed);
                Equipmentdb.SaveChanges();
                return Json(new { status = "true" });
            }
            else
                return Json(new { status = "false" });
        }

        [HttpPost]
        public ActionResult UserProfile(string Email)
        {
            foreach (tblUsers u in db.Users)
            {
                if (u.Email == Email)
                {
                    return Json(new { status = "true", lastName = u.LastName, role = u.Role });
                }
            }
            return Json(new { status = "false" });
        }

        [HttpPost]
        public ActionResult OrderRoom(string Email, string roomDate, string ItemName)
        {
            DateTime Date = DateTime.Parse(roomDate);
            if (Date < DateTime.Today) {
                return Json(new { status = "false", reason = "previusDate" });
            }

            foreach (tblRooms u in Roomsdb.Rooms)
            {
                if (u.Date == Date && u.ItemName== ItemName)
                {
                    return Json(new { status = "false", reason="occupied" });
                }
            }

            tblRooms l = new tblRooms();
            l.UserEmail = Email;
            l.ItemName = ItemName;
            l.Date = Date;
            Roomsdb.Rooms.Add(l);
            Roomsdb.SaveChanges();

            return Json(new { status = "true" });
        }

        [HttpPost]
        public ActionResult ReturnItem(string userEmail, string code, string startDate, string finalDate)
        {
            try
            {
                DateTime Start = DateTime.Parse(startDate);
                DateTime End = DateTime.Parse(finalDate);
                if (Start > DateTime.Today)
                {
                    return Json(new { status = "false" });
                }
                foreach (tblLoans u in Loansdb.Loans)
                {
                    if (u.UserEmail.Equals(userEmail) && u.StartDate == Start && u.FinalDate == End && u.ECode.Equals(code))
                    {
                        u.Returned = "T";
                    }
                }
                Loansdb.SaveChanges();
                return Json(new { status = "true" });
            }
            catch
            {
                return Json(new { status = "false" });
            }
        }      
         [HttpPost]
        public void AddNotification(string reason,string mail,string code,string body="")
        {
            int index = Notificationdb.Notification.Count() + 1;
            string AdminMail="";
            foreach(tblUsers u in db.Users)
            {
                if (u.Role.Equals("A"))
                {
                    AdminMail = String.Copy(u.Email);
                }
            }
            if (reason.Equals("LateWarning"))
            {
                if (!CheckNotificationExists(mail, code, DateTime.Today, "Notice! - reminder"))
                {
                    AddReminderNotification(mail, code, index);
                }
            }
            if (reason.Equals("LateLoan"))
            {
                if (!CheckNotificationExists(AdminMail, code, DateTime.Today, "Item not returned"))
                {
                    AddLateLoanNotification(AdminMail,mail, code, index);
                }
            }
            if (reason.Equals("Malfunction"))
            {
                AddMalfunctionNotification(AdminMail,mail, code, index, body);
            }
        }
         public void AddMalfunctionNotification(string Amail,string Umail, string code, int index,string body)
        {
            tblNotification l = new tblNotification();
            l.Code = code;
            l.Title = "Malfunction";
            l.Date = DateTime.Today;
            l.Body = Umail+" report on item "+code+": "+body;
            l.Email = Amail;
            l.Snumber = index;
            Notificationdb.Notification.Add(l);
            Notificationdb.SaveChanges();
        }
        [HttpPost]
        public ActionResult LateNotification(string userEmail)
        {
            try
            {
                foreach (tblLoans u in Loansdb.Loans)
                {
                    if (u.UserEmail.Equals(userEmail))
                    {
                        if (u.FinalDate == DateTime.Today.AddDays(1))
                        {
                            AddNotification("LateWarning",userEmail,u.ECode);
                        }
                    }
                    if(u.FinalDate<DateTime.Today&& u.Returned.Equals("F"))
                    {
                         AddNotification("LateLoan", u.UserEmail, u.ECode);
                    }
                }
                return Json(new { status = "true" });
            }
            catch
            {
                return Json(new { status = "false" });
            }
        }
        [HttpPost]
        public ActionResult ShowNotification(string userEmail)
        {
            NotificationViewModel f = new NotificationViewModel();
            List<tblNotification> NotificationList = new List<tblNotification>();
            userEmail = userEmail.ToLower(); 
            foreach (tblNotification u in Enumerable.Reverse(Notificationdb.Notification))
            {
                if (u.Email.ToLower().Equals(userEmail))
                {
                    NotificationList.Add(u);
                }
            }
            f.ShowNotificatios = NotificationList.ToList();
            return Json(new { NotificationList });
        }
        public Boolean CheckNotificationExists(string mail,string code,DateTime date,string reason)
        {
            int index = Notificationdb.Notification.Count() + 1;
            if (index == 1) { return false; }
            else
            {
                foreach (tblNotification u in Notificationdb.Notification)
                {
                    if(reason.Equals("Item not returned"))
                    {
                        if (code.Equals(u.Code) && mail.Equals(u.Email) && reason.Equals(u.Title))
                        {
                            return true; ;
                        }
                    }
                    else
                    {
                        if (DateTime.Today == u.Date && code.Equals(u.Code) && mail.Equals(u.Email)&& reason.Equals(u.Title))
                        {
                            return true; ;
                        }
                    }
                }
                return false;
            }
        }
        public void AddReminderNotification(string mail,string code,int index)
        {
            tblNotification l = new tblNotification();
            l.Code = code;
            l.Title = "Notice! - reminder";
            l.Date = DateTime.Today;
            l.Body = "You have 1 more day to return the item";
            l.Email = mail;
            l.Snumber = index;
            Notificationdb.Notification.Add(l);
            Notificationdb.SaveChanges();
        }
        
        public void AddLateLoanNotification(string Amail,string Umail, string code, int index)
        {
            tblNotification l = new tblNotification();
            l.Code = code;
            l.Title = "Item not returned";
            l.Date = DateTime.Today;
            l.Body = Umail + " doesnt return the item: " + code;
            l.Email = Amail;
            l.Snumber = index;
            Notificationdb.Notification.Add(l);
            Notificationdb.SaveChanges();
        }
        
        [HttpPost]
        public void DeleteOldNotification()
        {
            foreach(tblNotification u in Notificationdb.Notification)
            {
                if (u.Date <= DateTime.Today.AddDays(-7))
                {
                    Notificationdb.Notification.Remove(u);
                }
            }
            Notificationdb.SaveChanges();
        }
        
    }

}

