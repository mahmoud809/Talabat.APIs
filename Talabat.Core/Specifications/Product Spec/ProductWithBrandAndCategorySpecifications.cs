using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Product_Spec
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {

        //This Constructor Will be Used to create an object , That Will be Use to get [all products].
        public ProductWithBrandAndCategorySpecifications():base()
        {
            AddIncludes();
        }

        //This Constructor Will be Used to create an object , That Will be Use to get [ a specific product by id ].
        public ProductWithBrandAndCategorySpecifications(int id):base(P => P.Id == id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
        }
    }
}
