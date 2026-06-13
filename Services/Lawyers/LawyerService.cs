using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Exceptions.BadRequest;
using Domain.Exceptions.NotFound;
using Domain.Exceptions.ServerError;
using Services.Abstractions.Lawyers;
using Services.Helper;
using Services.Specifications;
using Services.Specifications.Cases;
using Services.Specifications.Lawyers;
using Shared.Dtos.Dashboard;
using Shared.Dtos.Lawyers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Done

namespace Services.Lawyers
{
    public class LawyerService(IUnitOfWork _unitOfWork,
                               IMapper _mapper) : ILawyerService
    {
        public async Task<LawyerResponse?> GetLawyerInfoAsync(string lawyerId)
        {
            CheckLawyerIdentifierExistance(lawyerId);
            var lawyer = await CheckLawyerExistanceToReturnAsync(lawyerId);
            return _mapper.Map<LawyerResponse>(lawyer);             
        }



        #region Note
        // lawyer = _mapper.Map<Lawyer>(lawyerUpdateRequest); // Wrong mapping, should map to existing lawyer entity instead of creating a new one  
        #endregion
        public async Task UpdateAsync(string lawyerId, LawyerUpdateRequest lawyerUpdateRequest)
        {
            CheckLawyerIdentifierExistance(lawyerId);
            var lawyer = await CheckLawyerExistanceToReturnAsync(lawyerId);
            _mapper.Map(lawyerUpdateRequest, lawyer); // Correct mapping to update existing entity which tracked by EF Core. Remain other properties Which Not Exist In UpdateRequest Not Be Overwritten Stay The Same As In DB
            _unitOfWork.GetRepository<string, Lawyer>().Update(lawyer); // Optinal
            int result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
                throw new ServerErrorExceptionText("Failed to update lawyer info. Please try again later.");
        }



        public async Task UpdateProfilePictureAsync(string lawyerId, LawyerUpdateProfilePictureRequest lawyerUpdateProfilePictureRequest)
        {
            CheckLawyerIdentifierExistance(lawyerId);
            var lawyer = await CheckLawyerExistanceToReturnAsync(lawyerId);

            if (lawyerUpdateProfilePictureRequest.ProfilePicture is not null && lawyer.ProfilePictureUrl is not null) // Mean A File Is Uploaded And There Is An Old Picture To Delete
                LawyerImageHelper.DeleteProfilePicture(lawyer.ProfilePictureUrl, "lawyerprofile");

            if (lawyerUpdateProfilePictureRequest.ProfilePicture is not null) // Mean A File Is Uploaded
                lawyerUpdateProfilePictureRequest.ProfilePictureUrl = LawyerImageHelper.UploadProfilePicture(lawyerUpdateProfilePictureRequest.ProfilePicture, "lawyerprofile");

            _mapper.Map(lawyerUpdateProfilePictureRequest, lawyer);

            _unitOfWork.GetRepository<string, Lawyer>().Update(lawyer); // Optinal
            int result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
                throw new ServerErrorExceptionText("Failed to update profile picture. Please try again later.");
        }



        public async Task<DashboardResponse> MyDashboardAsync(string lawyerId)
        {
            CheckLawyerIdentifierExistance(lawyerId);

            var caseRepo = _unitOfWork.GetRepository<int, Case>();
            var sessionRepo = _unitOfWork.GetRepository<int, CourtSession>();
            var appealRepo = _unitOfWork.GetRepository<int, Appeal>();
            var decisionRepo = _unitOfWork.GetRepository<int, Decision>();

            var today = DateTime.Now.Date;
            var tomorrow = today.AddDays(1);
            var nextWeek = today.AddDays(7);
            var oneMonthAgo = DateTime.Now.AddMonths(-1);

            var MyDashboard = new DashboardResponse()
            {

                // We Build Queries On DataBase Direct Using Repository Without Download Data From DB To Memory As Include And Check On This Data Which In Memory
                // Here We Not Make This Here We Build Query On Database Direct

                // Case --> Lawyers
                // Check Case If Related To Lawyer Or Not By Checking If Any Lawyer In This Case Has Id Equal To LawyerId, Then Check Status Of This Case To Get Total Active, Closed And OnHold Cases Count
                TotalActiveCases = await caseRepo.CountAsync(C => C.Lawyers.Any(L => L.Id == lawyerId) && C.Status == CaseStatus.مفتوحة),
                TotalClosedCases = await caseRepo.CountAsync(C => C.Lawyers.Any(L => L.Id == lawyerId) && C.Status == CaseStatus.مغلقة),
                TotalOnHoldCases = await caseRepo.CountAsync(C => C.Lawyers.Any(L => L.Id == lawyerId) && C.Status == CaseStatus.معلقة),
                LastCasesAddedCountPerMonth = await caseRepo.CountAsync(C => C.Lawyers.Any(L => L.Id == lawyerId) && C.CreatedAt >= oneMonthAgo),

                // Session --> Case --> Lawyers
                // Check Session If Related To Lawyer Or Not By Checking If Any Lawyer In This Session's Case Has Id Equal To LawyerId, Then Check SessionDate To Get Upcoming Sessions Count Per Week And Today's Sessions Count
                TodaysSessionsCount = await sessionRepo.CountAsync(S => S.Case.Lawyers.Any(L => L.Id == lawyerId) && S.SessionDate >= today && S.SessionDate <= tomorrow),
                UpcomingSessionsCountPerWeek = await sessionRepo.CountAsync(S => S.Case.Lawyers.Any(L => L.Id == lawyerId) && S.SessionDate >= today && S.SessionDate <= nextWeek),

                // Decision --> Session --> Case --> Lawyers
                DecisionsWithAppealDeadlineThisWeek = await decisionRepo.CountAsync(D => D.CourtSession.Case.Lawyers.Any(L => L.Id == lawyerId) && D.AppealDeadline >= today && D.AppealDeadline <= nextWeek),

                // Appeal --> Decision --> Session --> Case --> Lawyers
                //UnderReviewedAppealsCount = await appealRepo.CountAsync(A => A.OriginalDecision.CourtSession.Case.Lawyers.Any(L => L.Id == lawyerId) && A.Status == AppealStatus.UnderReview && A.Outcome == AppealOutcome.Pending)
            };
            return MyDashboard;
        }



        // Need Modify In Feature
        public async Task<IEnumerable<DecisionResponseForDashboard>> GetDecisionsWithAppealDeadlineThisWeekAsync(string lawyerId)
        {
            CheckLawyerIdentifierExistance(lawyerId);

            var today = DateTime.Now.Date;
            var nextWeek = today.AddDays(7);

            var decisionSpecifications = new DecisionSpecificationsDashboard(lawyerId, today, nextWeek, true, true);
            var decisions = await _unitOfWork.GetRepository<int, Decision>().GetAllAsync(decisionSpecifications);
            return _mapper.Map<IEnumerable<DecisionResponseForDashboard>>(decisions);
        }





        #region Helper_Methods
        private void CheckLawyerIdentifierExistance(string? lawyerId)
        {
            // 1. Check LawyerId
            if (string.IsNullOrEmpty(lawyerId))
                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");
        }

        private async Task<Lawyer> CheckLawyerExistanceToReturnAsync(string lawyerId)
        {
            var specifications = new LawyerSpecifications(lawyerId, false);
            var lawyer = await _unitOfWork.GetRepository<string, Lawyer>().GetByIdAsync(specifications);
            if (lawyer is null)
                throw new LawyerNotFoundException("Lawyer not found");
            return lawyer;
        }
        #endregion









        #region Description_For_New_Dashboard
        // Go Backward
        // I Work On Database Level To Get Required Data Related To Lawyer To Put It In Dashboard Response Object By Using Repositories And Filtering Data By LawyerId And Required Conditions To Get Each Count,
        // Instead Of Getting Lawyer Then His Cases Then Sessions Of This Cases Then Decisions Of This Sessions Then Appeals Of This Decisions To Get All Required Data Related To Lawyer To Put It In Dashboard Response Object,
        // I Will Use CaseRepo Then Filter It By LawyerId And Status To Get Total Active, Closed And OnHold Cases Count,
        // Then Get All Sessions Then Get Her Case Then Filter It By LawyerId And SessionDate To Get Upcoming Sessions Count Per Week And Today's Sessions Count,
        // Then Get All Cases Again Then Filter It By LawyerId And CreatedAt To Get Last Cases Added Count Per Month,
        // Then Get All Appeals Then Filter It By LawyerId And Appeal Status To Get Under Reviewed Appeals Count, Then Get All Decisions Then Filter It By LawyerId And Appeal Deadline To Get Decisions With Appeal Deadline This Week Count 
        #endregion


        #region Description_For_Old_Dashboard
        // Go Forward 
        // Get Lawyer Then His Cases Then Sessions Of This Cases Then Decisions Of This Sessions Then Appeals Of This Decisions To Get All Required Data Related To Lawyer To Put It In Dashboard Response Object 
        #endregion

        #region Old_Dashboard
        //var lawyerSpecifications = new LawyerSpecifications(lawyerId, true);
        //var lawyer = await _unitOfWork.GetRepository<string, Lawyer>().GetByIdAsync(lawyerSpecifications);
        //int ActiveCasesCount = GetActiveCasesCountForSpecificLawyer(lawyer);
        //int ClosedCasesCount = GetClosedCasesCountForSpecificLawyer(lawyer);
        //int OnHoldedCasesCount = GetOnHoldedCasesCountForSpecificLawyer(lawyer);
        //int UpcomingSessionsCountPerWeek = GetUpcomingSessionsCountPerWeek(lawyer);
        //int TodaysSessionsCount = GetTodaysSessionsCount(lawyer);
        //int LastCasesAddedCountPerMonth = GetLastCasesAddedCountPerMonth(lawyer);
        //int UnderReviewedAppealsCount = GetUnderReviewedAppealsCount(lawyer);
        //int DecisionsWithAppealDeadlineThisWeekCount = GetDecisionsWithAppealDeadlineThisWeekCount(lawyer); 
        #endregion

        #region Old_Dashboard
        //private int GetActiveCasesCountForSpecificLawyer(Lawyer lawyer)
        //{
        //    int ActiveCasesCount = 0;
        //    if (lawyer is not null)
        //        ActiveCasesCount = lawyer.Cases.Where(C => C.Status == CaseStatus.Active).Count();
        //    return ActiveCasesCount;
        //}

        //private int GetClosedCasesCountForSpecificLawyer(Lawyer lawyer)
        //{
        //    int ClosedCasesCount = 0;
        //    if (lawyer is not null)
        //        ClosedCasesCount = lawyer.Cases.Where(C => C.Status == CaseStatus.Closed).Count();
        //    return ClosedCasesCount;
        //}

        //private int GetOnHoldedCasesCountForSpecificLawyer(Lawyer lawyer)
        //{
        //    int OnHoldedCasesCount = 0;
        //    if (lawyer is not null)
        //        OnHoldedCasesCount = lawyer.Cases.Where(C => C.Status == CaseStatus.OnHold).Count();
        //    return OnHoldedCasesCount;
        //}

        //private int GetUpcomingSessionsCountPerWeek(Lawyer lawyer)
        //{
        //    var allSessions = lawyer.Cases.SelectMany(c => c.CourtSessions);
        //    DateTime startRange = DateTime.Now;
        //    DateTime endRange = DateTime.Now.AddDays(7);
        //    int UpcomingSessionsCountPerWeek = allSessions.Where(Session =>
        //                                                         Session.SessionDate >= startRange &&
        //                                                         Session.SessionDate <= endRange)
        //                                                  .Count();
        //    return UpcomingSessionsCountPerWeek;
        //}

        //private int GetTodaysSessionsCount(Lawyer lawyer)
        //{
        //    var allSessions = lawyer.Cases.SelectMany(c => c.CourtSessions);
        //    DateTime startRange = DateTime.Now;
        //    DateTime endRange = DateTime.Now.AddDays(1);
        //    int TodaysSessionsCount = allSessions.Where(Session =>
        //                                                         Session.SessionDate >= startRange &&
        //                                                         Session.SessionDate <= endRange)
        //                                         .Count();
        //    return TodaysSessionsCount;
        //}

        //private int GetLastCasesAddedCountPerMonth(Lawyer lawyer)
        //{
        //    var allCases = lawyer.Cases;
        //    DateTime startRange = DateTime.Now.AddMonths(-1);
        //    DateTime endRange = DateTime.Now;
        //    int LastCasesAddedCountPerMonth = allCases.Where(c =>
        //                                                         c.CreatedAt >= startRange &&
        //                                                         c.CreatedAt <= endRange)
        //                                              .Count();
        //    return LastCasesAddedCountPerMonth;
        //}

        //private int GetUnderReviewedAppealsCount(Lawyer lawyer)
        //{  
        //    var allAppeals = lawyer.Cases.SelectMany(Case => Case.CourtSessions)
        //                                 .SelectMany(Session => Session.Decisions)
        //                                 .SelectMany(Decision => Decision.Appeals);
        //    int UnderReviewedAppealsCount = allAppeals.Where(Appeal => Appeal.Status == AppealStatus.UnderReview)
        //                                              .Count();
        //    return UnderReviewedAppealsCount;
        //}

        //private int GetDecisionsWithAppealDeadlineThisWeekCount(Lawyer lawyer)
        //{
        //    var allDecisions = lawyer.Cases.SelectMany(Case => Case.CourtSessions)
        //                                   .SelectMany(Session => Session.Decisions);
        //    DateTime startRange = DateTime.Now;
        //    DateTime endRange = DateTime.Now.AddDays(7);
        //    int DecisionsWithAppealDeadlineThisWeekCount = allDecisions.Where(Decision => 
        //                                                                      Decision.AppealDeadline >= startRange &&
        //                                                                      Decision.AppealDeadline <= endRange)
        //                                                               .Count();
        //    return DecisionsWithAppealDeadlineThisWeekCount;
        //} 
        #endregion

    }
}
