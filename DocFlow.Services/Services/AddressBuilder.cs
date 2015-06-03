using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocFlow.Core.Domain;
using Services.Interfaces;

namespace FileTest.Services
{
    public class DefaultAddressBuilder : IAddressBuilder
    {
        public const string PathBuildingFormat = "V:/Employees/Active Employees/{LastName}, {FirstName}/{DirectoryType}/";
        public const string FileNameFormat = "{DocumentType}{DocumenbtDate}";

        public string BuildDestination(IEnumerable<FormField> fields, string fileExtenion)
        {            
            string path = PathBuildingFormat;
            string fileName = FileNameFormat;

            foreach (var field in fields)
            {
                if (path.Contains(field.Name))
                {
                    path.Replace("{" + field.Name + "}", field.Response);
                }


                if (fileName.Contains(field.Name))
                {
                    fileName.Replace("{" + field.Name + "}", field.Response);
                }
            }

            return string.Format("{0}{1}.{2}", path, fileName, fileExtenion);
        }

        public string BuildDestination(IEnumerable<FormField> fields)
        {
            throw new NotImplementedException();
        }
    }
}
