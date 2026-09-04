using System;
using DATA;

namespace BIZ.Entity
{
    [Serializable]
    public class MANUFACTORY_FULL: Manufactory
    {
        public Manufactory ConvertToBase ()
        {
            Manufactory manufactory = new Manufactory ();
            manufactory.Id = Id;
            manufactory.Title = Title;
            manufactory.Description = Description;
            manufactory.Image = Image;
            manufactory.Website = Website;
            manufactory.Published = Published;
            manufactory.Ordering = Ordering;
            manufactory.Params = Params;

            return manufactory;
        }
    }
}
