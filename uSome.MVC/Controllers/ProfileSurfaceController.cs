using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Profile;
using umbraco.cms.businesslogic.member;
using Umbraco.Web.Mvc;
using uSome.Membership.Models;
using System.Web.Mvc.Html;

namespace uSome.Membership.Controllers
{
    public class ProfileSurfaceController : SurfaceController
    {
       

        [HttpPost]
        public ActionResult profile(ProfileModel profileModel,HttpPostedFileBase Image, int memberId)
        {
            //use this namespace => umbraco.cms.businesslogic.Files.UmbracoFile;
            //using umbraco.editorControls.SettingControls.Pickers;

            try 
            {
                var vProfileDirectoryPath = Server.MapPath(string.Format("/MemberProfile/{0}/", memberId));

                if (!Directory.Exists(vProfileDirectoryPath))
                {
                    Directory.CreateDirectory(vProfileDirectoryPath);
                }
              
                if (User.Identity.IsAuthenticated)
                {
                    var member = Member.GetCurrentMember();
                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(vProfileDirectoryPath, fileName);
                            file.SaveAs(path);
                            profileModel.Image =fileName;
                        }
                    }

                    member.getProperty("image").Value = profileModel.Image;
                    member.getProperty("gender").Value = profileModel.Gender;
                    member.getProperty("relationToDepression").Value = profileModel.RelationToCancer;
                    member.getProperty("dateOfBirth").Value = profileModel.Dob;
                    member.Save();
                    TempData["ProfileCreationSuccessMessage"] = umbraco.library.GetDictionaryItem("UMF_ProfileCreatedSuccess");
                    return RedirectToCurrentUmbracoPage();
                }
                else
                {
                    return Redirect("/");
                }
            }
            catch(Exception ex)
            {
                TempData["ProfileCreationFailureMessage"] = umbraco.library.GetDictionaryItem("UMF_ProfileCreationFailureMessage");
                return RedirectToCurrentUmbracoPage();
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MoreAboutMe(MoreAboutMeModel model)
        {
            var member = Member.GetCurrentMember();
            try
            {
                if (member!=null)
                {
                    member.getProperty("motto").Value = model.Motto;
                    member.getProperty("shortBiography").Value = model.ShortBiography;
                    member.getProperty("accommodation").Value = model.Accommodation;
                    member.getProperty("everyDayLifeDuty").Value = model.EveryDayLifeDuty;
                    member.getProperty("province").Value = model.Province;
                    member.Save();
                    TempData["AdditionalInformationCreationSuccessMsg"] = umbraco.library.GetDictionaryItem("UMF_AdditionalInformationPassMSg");
                    return RedirectToCurrentUmbracoPage(); 
                }
                TempData["AdditionalInformationCreationFailureMsg"] = umbraco.library.GetDictionaryItem("UMF_AdditionalInformationFailMSg");
                return RedirectToCurrentUmbracoPage();

            }
            catch (Exception)
            {
                TempData["AdditionalInformationCreationFailureMsg"] = umbraco.library.GetDictionaryItem("UMF_AdditionalInformationFailMSg");
                return RedirectToCurrentUmbracoPage();
            }
        }


        [HttpPost]
        public ActionResult AddResearch(ResearchModel model, int UserId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var member = Member.GetCurrentMember();
                    if (member != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var item in model.InvestigationList)
                        {
                            var existedValue = member.getProperty("investigation").Value.ToString();
                            if (existedValue != "")
                            {
                                sb.Append(existedValue + "," + item + ",");
                            }
                            else
                            {
                                sb.Append(item + ",");
                            } 
                        }
                        var val = sb.ToString().TrimEnd(',');
                        member.getProperty("investigation").Value = val;
                        member.Save();
                        return RedirectToCurrentUmbracoPage();

                    }
                }
                catch (Exception ex)
                {
                    return CurrentUmbracoPage();
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddConsequences(ConsequencesModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var member = Member.GetCurrentMember();
                    if (member != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var item in model.ConsequencesList)
                        {
                            var existedValue = member.getProperty("consequences").Value.ToString();
                            if (existedValue != "")
                            {
                                var values = existedValue.Split(',');
                                sb.Append(existedValue + " ," + item + " ,");
                            }
                            else
                            {
                                sb.Append(item + " ,");
                            }
                        }
                        var val = sb.ToString().TrimEnd(',');
                        member.getProperty("consequences").Value = val;
                        member.Save();
                        return RedirectToCurrentUmbracoPage();

                    }
                }
                catch (Exception ex)
                {
                    return CurrentUmbracoPage();
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddAdjustVisibility(VisibilityModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var member = Member.GetCurrentMember();
                    if (member != null)
                    {
                        member.getProperty("visibility").Value = model.visibility;
                        member.Save();
                    }

                }
                catch (Exception)
                {
                    return CurrentUmbracoPage();
                }

            }
            return CurrentUmbracoPage();
        }
    }
}
