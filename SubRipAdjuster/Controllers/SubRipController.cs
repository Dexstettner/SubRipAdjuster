using SubRipAdjuster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Web;

namespace SubRipAdjuster.Controllers
{
    public class SubRipController : Controller
    {
        public ActionResult SubRipForm()
        {
            return View();
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
                SubRipFile srf = new SubRipFile(file);
                srf.Add(offset);

                Thread t = new Thread((ThreadStart)delegate
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "SubRip Files (*.srt)|*.srt|All files (*.*)|*.*";
                    sfd.DefaultExt = ".srt";
                    sfd.FilterIndex = 2;
                    sfd.RestoreDirectory = true;

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        srf.SaveAs(Path.GetFullPath(sfd.FileName));
                        ModelState.AddModelError("Sucesso", "Arquivo salvo com sucesso em: " + Path.GetFullPath(sfd.FileName));
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Problema ao acessar diretorio" + Path.GetFullPath(sfd.FileName));
                    }
                });

                t.TrySetApartmentState(ApartmentState.STA);

                //start the thread 
                t.Start();

                // Wait for thread to get started 
                while (!t.IsAlive) { Thread.Sleep(1); }
                // Wait a tick more
                Thread.Sleep(1);
                //wait for the dialog thread to finish 
                t.Join();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }
            return View();

        }
    }
}