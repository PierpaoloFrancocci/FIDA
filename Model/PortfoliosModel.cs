using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FidaWorkstation.Professional.ObjectModel;
using FidaWorkstation.Professional.Data.Service.Interfaces;
using System.Web.Mvc;


namespace FidaWorkstation.Professional.Web.Models
{
    public class DDLMustBeSelected : ValidationAttribute
    {
        public override bool IsValid(object propertyValue)
        {

            if (null == propertyValue) return true;     //not a required validator
          
            return propertyValue != null
                && propertyValue is bool
                && (bool)propertyValue;
        }


    }
    public class InterviewPortfoglioViewModel 
    {
        [DisplayName("Tipo portfoglio")]
        [Required(ErrorMessage = "Devi selezionare il tipo portfoglio.")]
        [DDLMustBeSelected(ErrorMessage = "You must select one item")]
        public Int64 TipoPortfoglio { get; set; }


        public Int64 PortfoglioID { get; set; }

        public IEnumerable<SelectListItem> TipoPortfoglioList { get; set; }

        [DisplayName("Nome portfoglio")]
        [Required(ErrorMessage = "Nome portfoglio è richiesto.")]
        public string NamePortfoglio { get; set; }

        [DisplayName("Descrizione portfoglio")]
        public string DescriptionPortfoglio { get; set; }

        [DisplayName("Valuta")]
        [Required(ErrorMessage = "Devi selezionare la valuta portfoglio.")]
        public string Currency { get; set; }

        public IEnumerable<SelectListItem> CurrencyList { get; set; }

        [DisplayName("Periodo di bilanciamento")]
        [Required(ErrorMessage = "Periodo di bilanciamento del portfoglio è richiesta.")]
        public string PeriodoBilanciamento { get; set; }
        public IEnumerable<SelectListItem> PeriodoBilanciamentoList { get; set; }

        [DisplayName("Start Date")]
        [Required(ErrorMessage = "Start date del portfoglio è richiesta.")]
        public DateTime StartDate { get; set; }

        public string UserID { get; set; }

        public InterviewPortfoglioViewModel()
        {
            var list = new List<SelectListItem>() {
                new SelectListItem { Text = "(Select)" },
                new SelectListItem { Value = "1", Text = "Tipo ASSETALLOCATION" },
                new SelectListItem { Value = "2", Text = "Tipo ASSETALLOCATION BILANCIATO" },
                new SelectListItem { Value = "3", Text = "Tipo CONTABILE" }
            };
            this.TipoPortfoglioList = new SelectList(list, "Value", "Text");

            list = new List<SelectListItem>() {
                new SelectListItem { Text = "(Select)" },
                new SelectListItem { Value = "1", Text = "EUR" },
                new SelectListItem { Value = "2", Text = "USD" },
                new SelectListItem { Value = "3", Text = "GBP" },
                new SelectListItem { Value = "4", Text = "CHF" }
            };
            this.CurrencyList= new SelectList(list, "Value", "Text");
            list = new List<SelectListItem>() {
                //new SelectListItem { Text = "(Select)" },
                new SelectListItem { Value = "1", Text = "MENSILE" },
                new SelectListItem { Value = "2", Text = "BIMESTRALE" },
                new SelectListItem { Value = "3", Text = "TRIMESTRALE" },
                new SelectListItem { Value = "4", Text = "QUADRIMESTRALE" },
                new SelectListItem { Value = "6", Text = "SEMESTRALE" },
                new SelectListItem { Value = "12", Text = "ANNUALE" }
                
            };
            this.PeriodoBilanciamentoList = new SelectList(list, "Value", "Text");
        }
    }

    public class PortFoglioTreeViewModel
    {
        public IList<pfoglioDTO> portfoglioDTO { get; set; }

        public PortFoglioTreeViewModel()
        {
            this.portfoglioDTO = new List<pfoglioDTO>();
        }
    }



    public class PortFoglioViewModel
    {
        public IList<pfoglioDTO> portfoglioDTO { get; set; }

        public PortFoglioViewModel()
        {
            this.portfoglioDTO = new List<pfoglioDTO>();
        }
    }


    public class pfoglioTreeDTO
    {
        public IList<CustomerTree> Customer { get; set; }

        public pfoglioTreeDTO()
        {
            this.Customer = new List<CustomerTree>();
        }
    }
    public class CustomerTree
    { 
        public IList<pFtreeDTO> lp { get; set; }

        public UserProfile up { get; set; }
        public Guid gUserID { get; set; }
        public CustomerTree()
        {
            this.lp = new List<pFtreeDTO>();
        }
    
    }
    public class pFtreeDTO
    {
        
        public PortfolioInfoDTO pfInfo { get; set; }
        public IList<pfOperationDTO> lop { get; set; }
        public pFtreeDTO()
        {
            this.lop = new List<pfOperationDTO>();
        }
    }


    public class WLTreeDTO
    {
        public IList<WLTree> WL { get; set; }

        public WLTreeDTO()
        {
            this.WL = new List<WLTree>();
        }
    }
    public class WLTree
    {
        public IList<pfOperationDTO> lop { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public long  IDWL { get; set; }
        public WLTree()
        {
            this.lop = new List<pfOperationDTO>();
        }

    }





    public class pfOperationDTO
    {
        public string FundName { get; set; }
        public PortfolioOperationDTO pfOpDTO { get; set; }
    }


    public class pfoglioDTO
    {
        public IList<PortfolioInfoDTO> lp { get; set; }
        public UserProfile up { get; set; }
        public Guid gUserID { get; set; }
        public pfoglioDTO()
        {
            this.lp = new List<PortfolioInfoDTO>();
        }

    }


    //models per dettaglio portafoglio
    public class PortFoglioDetailViewModel
    {
        public PortfolioInfoDTO pfinfo { get; set; }
        public UserProfile up { get; set; }
        public IList<PortfolioOperationDTO> lpo { get; set; }

        public PortFoglioDetailViewModel() 
        {
            this.lpo = new List<PortfolioOperationDTO>();
        }
    }
    public class PortFoglioDetailFullModel
    {
        public PortFoglioDetailViewModel pfDetail { get; set; }
        public FWSDataTable fwsData { get; set; }
        public FWSHistDataTable fwsHist { get; set; }
        public Dictionary<string, string> fc2Name { get; set; }
        public Double pfValue { get; set; }
        public NAVCalculatorDTO NavDTO { get; set; }
    }


    public class ListDDLItem
    {
        public IList<DDLItem> lItems { get; set; }
        public ListDDLItem()
        {
            this.lItems = new List<DDLItem>();
        }

    }


    public class DDLItem
    {
        public Int32 ID { get; set; }
        public string DESC { get; set; }

        public DDLItem()
        {          
        }

    }

    public class JsonResponse
    {
        public String _Response { get; set; }
    }

    public class PtfOperation
    {
        public int NumOperation { get; set; }

        public Int64 _portfolioListId { get; set; }
        public String _fidaCode { get; set; }
        public String _fundName { get; set; }
        public string _operationDate { get; set; }
        public Double _quantity { get; set; }
        public Double _unitPrice { get; set; }
        public Double _price { get; set; }
        public Double _weight { get; set; }
    }

    public class PtfInfoPrize
    {
        public string unitPriceDate { get; set; }
        public Double unitPrice { get; set; }
    }

    public class wlInfo
    {
        public Int64 _watchlistListID { get; set; }
        public Int64 _watchlistID { get; set; }
        public string _fidaCode { get; set; }
        public string _fundName { get; set; }
    }



}