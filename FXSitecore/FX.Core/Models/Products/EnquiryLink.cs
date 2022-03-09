using Sitecore.Data.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.Products
{
	public class EnquiryLink
	{
		public bool IsEnquiryForm { get; set; }
		public string Url { get; set; }
		public string Label { get; set; }

		public EnquiryLink() { }

		public EnquiryLink(FX.Core.Models.Products.IProductPage productPage)
		{
			if (productPage != null)
			{
				this.IsEnquiryForm = productPage.ProductForm != null;
				this.Label = productPage.ProductFormLinkText;

				if (this.IsEnquiryForm)
				{
					this.Url = FX.Core.Utils.Util.GetNormalizedId(productPage.ProductForm.Id);
				}
				else if (FX.Core.Utils.Util.HasValidLink(productPage.ProductLink))
				{
					this.Url = productPage.ProductLink.Url;
					if (!string.IsNullOrWhiteSpace(productPage.ProductLink.Text))
					{
						this.Label = productPage.ProductLink.Text;
					}
				}
			}
		}

		public EnquiryLink(FX.Core.Search.Models.FXResultItem resultItem)
		{
			if (resultItem != null)
			{
				this.IsEnquiryForm = !Sitecore.Data.ID.IsNullOrEmpty(resultItem.ProductForm);
				this.Label = resultItem.ProductFormLinkText;
				if (this.IsEnquiryForm)
				{
					this.Url = FX.Core.Utils.Util.GetNormalizedId(resultItem.ProductForm.Guid);
				}
				else if (resultItem.ProductButtonLink != null)
				{
					this.Url = $"/{FX.Core.Utils.Util.OpcoType()}{resultItem.ProductButtonLink.Url}";
					if (!string.IsNullOrWhiteSpace(resultItem.ProductButtonLink.Text))
					{
						this.Label = resultItem.ProductButtonLink.Text;
					}
				}
			}
		}

		public string ButtonType { get { return IsEnquiryForm ? "modalbox" : "ext-link"; } }
	}
}
