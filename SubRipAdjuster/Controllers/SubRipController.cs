using SubRipAdjuster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Web;
using SubRipAdjuster.DAL;
using System.Text;

namespace SubRipAdjuster.Controllers
{
    public class SubRipController : Controller
    {
        private SubripDatabaseContext db = new SubripDatabaseContext();
        public ActionResult SubRipForm()
        {
            return View("SubRipForm");
        }

        [HttpPost]

        public ActionResult SubRipForm(HttpPostedFileBase file, int offset = 0)
        {

            //Validate if the file was uploaded
            if (object.ReferenceEquals(null, file))
            {
                ModelState.AddModelError("FileError", "Selecione um arquivo");
                return View();
            }
            //Validate if file extension is .srt
            if (!Path.GetExtension(file.FileName).Equals(".srt"))
            {
                ModelState.AddModelError("FileError", "Formato do arquivo deve ser .srt");
                return View();
            }

            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            //Validate if offset is not null
            if (offset == 0)
            {
                ModelState.AddModelError("OffsetError", "Valor não pode ser 0 ou nulo");
                return View();
            }


            try
            {
                SubRipFile srf = new SubRipFile(SubRipFile.FileToByte(file));
                srf.Add(offset);

                Tuple<String, String, String> status = srf.OpenDialogSaveAs();
                ModelState.AddModelError(status.Item1, status.Item2);

                //Adiciona ao banco
                db.ArquiveHistory.Add(new ArquiveHistory(Path.GetFileName(status.Item3), DateTime.Now, SubRipFile.StringToByte(srf.Render()), offset));
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }
            return View();

        }

        public ActionResult SubRipHistory()
        {
            return View("SubRipHistory", db.ArquiveHistory.ToList());
        }

        [HttpPost]
        public ActionResult SubRipHistory(int id)
        {
            if (id > 0)
            {
                ArquiveHistory arquive = db.ArquiveHistory.Find(id);
                SubRipFile srf = new SubRipFile(arquive.ArquiveFile);
                Tuple<String, String, String> status = srf.OpenDialogSaveAs();
                ModelState.AddModelError(status.Item1, status.Item2);
            }
            return View(db.ArquiveHistory.ToList());
        }
    }
}