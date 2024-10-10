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
        public ProductWithBrandAndCategorySpecifications(string? sort , int? brandId , int? categoryId)
            :base(P => 
                            (!brandId.HasValue || P.BrandId == brandId.Value) &&
                            (!categoryId.HasValue || P.CategoryId == categoryId.Value)
                 )
        {
            AddIncludes();

            if(!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "priceAsc" :
                        AddOrderBy(P => P.Price);
                        break;
                    
                    case "priceDesc" :
                        AddOrderByDesc(P => P.Price);
                        break;
                  
                    default :
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else 
            {
                AddOrderBy(P => P.Name);
            }
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
