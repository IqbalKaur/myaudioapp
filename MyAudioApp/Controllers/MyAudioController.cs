using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MyAudioApp.Models;

namespace MyAudioApp.Controllers
{
    public class MyAudioController : Controller
    {
        private MyAudioDBEntities db = new MyAudioDBEntities();
        //
     /*   // GET: /MyAudio/About

        public ActionResult About()
        {
            return View();
        }
        //
        //GET: /MyAudio/contact
        public ActionResult Contact()
        {
            return View();
        } */
        //
        // GET: /MyAudio/ 

        public ActionResult Index(string searchstring = "")
        {
           // searchstring = "proper";
            var audio = from m in db.MyAudioTBs select m;
            if (!String.IsNullOrEmpty(searchstring))
            {
                //movies = movies.Where(s => s.Title.Contains(searchString));
                audio = audio.Where(s => s.Song.Contains(searchstring));
            }
            return View(audio);
        }

        //
        // GET: /MyAudio/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            MyAudioTB audio = db.MyAudioTBs.Find(id);
            if(audio == null )
            {
                return HttpNotFound();
            }
            return View(audio);
        }

        //
        // GET: /MyAudio/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MyAudio/Create

       // [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([Bind(Exclude = "id")] MyAudioTB audiotocreate) 
        {
            if(!ModelState.IsValid )
            return View();
            db.MyAudioTBs.Add(audiotocreate);
            db.SaveChanges();
            return RedirectToAction("Index");    
        }
        //
        // GET: /MyAudio/Uploadsong/
       
            [HttpPost] 
        public ActionResult Upload(HttpPostedFileBase file) 
            { 
                try 
                { 
                    if (file.ContentLength > 0)
                {
                    var filename = System.IO.Path.GetFileName(file.FileName);
                    //var fileName =Path.GetFileName(file.FileName); 
                  //var path =  Path.Combine(Server.MapPath("~/App_Data/Audio"), filename ); 
                       
                       var path = System.IO.Path.Combine(Server.MapPath ("~/Audio"),filename);
                       file.SaveAs(path);
                    } 
                    ViewBag.Message = "Upload successful";
                    return RedirectToAction("Index");
                } 
                catch
                {
                    ViewBag.Message = "Upload failed"; 
                    return RedirectToAction("Index"); 
                }
            } 
            
        //
        // GET: /MyAudio/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MyAudioTB audiotoedit = db.MyAudioTBs.Find(id);
            if(audiotoedit == null )
            {
                return HttpNotFound();
            }
            //var audiotoedit = (from m in db.MyAudioTBs where m.Id == id select m).First();

            return View(audiotoedit);
        }

        //
        // POST: /MyAudio/Edit/5

        [HttpPost]
        public ActionResult Edit(MyAudioTB audiotoedit)
        {
            //var originalaudio = (from m in db.MyAudioTBs where m.Id == audiotoedit.Id  select m).First();
            if (ModelState.IsValid)
            {
                //return View(originalaudio);
                // _db.ApplyPropertyChanges(originalMovie.EntityKey.EntitySetName, movieToEdit);
                db.Entry(audiotoedit).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(audiotoedit);
        }

        //
        // GET: /MyAudio/Delete/5

        public ActionResult Delete(int? id)
        {
            if(id == null )
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            MyAudioTB audio = db.MyAudioTBs.Find(id);
            if(audio == null )
            {
                return HttpNotFound();
            }

            return View(audio);
        }

        //
        // POST: /MyAudio/Delete/5

        [HttpPost,ActionName ("Delete")]
    [ValidateAntiForgeryToken ]
        public ActionResult DeleteConfirmed(int id)
        {
            MyAudioTB audio = db.MyAudioTBs.Find(id);
            db.MyAudioTBs.Remove(audio);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Player()
        {
           // Response.AppendHeader("Content-Disposition", "inline; filename=music.wav");
            //return File(audioFilename, "audio/wav");
            return View();
        }
      

    }
}
