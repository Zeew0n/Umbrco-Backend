using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using umbraco.cms.businesslogic.member;
namespace uSome.Utilities
{
    public class TreatmentController : SurfaceController
    {
        Treatment t = new Treatment();


        public JsonResult AddTreatment(int treatmentTypeId, int hospitalId, DateTime treatmentDate, int userId, int id)
        {
                try
                {
                    var model = new TreatmentModel
                    {
                        TreatmentTypeId = treatmentTypeId,
                        HospitalId= hospitalId,
                        TreatmentDate = treatmentDate,
                        UserId = userId,
                        Id = id
                    };
                    t.Save(model);
                    return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    Log.ErrorLog("Error in Treatment :: " + ex.Message);
                    return Json(new { result = "fail" }, JsonRequestBehavior.AllowGet);
                }
        }
        public ActionResult DeleteTreatment(int id)
        {
            try
            {
                t.Delete(id);
                return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.ErrorLog("Error in Treatment :: " + ex.Message);
                return Json(new { result = "fail" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UpdateMemberTreatment(TreatmentModel model)
        {
           
                try
                {
                    var member = Member.GetCurrentMember();
                    member.getProperty("treatmentPhase").Value = model.TreatmentPhase;
                    member.getProperty("isInterested").Value = model.IsInterested;
                    member.Save();
                    TempData["SuccessMessage"] = "Treatment Methodology details Saved";
                    return RedirectToCurrentUmbracoPage();
                }
                catch (Exception ex)
                {
                    Log.ErrorLog("Error in Treatment :: " + ex.Message);
                    TempData["ErrorMessage"] = "Treatment not saved";
                    return CurrentUmbracoPage();
                }
        }
    }
}
