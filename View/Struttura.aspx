<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="FidaWorkstation.Professional.Web" %>
<%@ Import Namespace="FidaWorkstation.Professional.Web.Models" %>
<%@ Import Namespace="FidaWorkstation.Professional.Web.Utils" %>
<%@ Import Namespace="FidaWorkstation.Professional.Data.Service.Interfaces" %>
<%@ Import Namespace="FidaWorkstation.Professional.ObjectModel" %>



<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Struttura
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%--<script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>--%>
    
    
    <link type="text/css" rel="stylesheet" href="../../content/JqueryUICust/jquery-ui-1.8.13.custom.css"  />
    <%--<script  type="text/javascript" src="../../Scripts/jquery-1.5.1.min.js"></script>--%>
	<script  type="text/javascript" src="../../Scripts/jquery.ui.core.js"></script>
	<script  type="text/javascript" src="../../Scripts/jquery.ui.widget.js"></script>
	<script  type="text/javascript" src="../../Scripts/jquery.ui.datepicker.js"></script>
	
	<script  type="text/javascript" src="../../Scripts/jquery.format-1.1.js"></script>
	
	
	<script src="../../Scripts/fidaWS.main.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.table.addrow.js" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.json-2.2.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.blockUI.js?v2.38") %>" type="text/javascript"></script> 
    
    
    
    <link rel="Stylesheet" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.7.2/themes/ui-darkness/jquery-ui.css" type="text/css" />
    
    <link type="text/css" rel="Stylesheet" href="<%= Url.Content("~/content/ui.selectmenu.css") %>"  />
    
	
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.7.2/jquery-ui.min.js"></script>
	
	<script src="<%= Url.Content("~/Scripts/ui.selectmenu.js") %>" type="text/javascript" ></script>
	
	
	
	
    
    
    <style type="text/css">
         
        #main
            {
                padding: 10px 10px 10px 10px;
                background-color: #fff;
                /*margin-bottom: 30px; */
                _height: 1px; /* only IE6 applies CSS properties starting with an underscore */
            }
        
        div.blockMe { padding: 0px; margin: 0px; background-color: #fff }
        
        .atable{
	        border-collapse:collapse;
	        border:1px solid #AAA;
	        /*margin-left:10px;*/
        }
       /*.atable th{
	        border:1px solid #AAF;
	        background:#BFBFFF;
	        font-weight:bold;
        }*/
        .atable td{
	        padding:10px;
	        border:1px solid #AAF;
        }
        .atable td button {
          width:10%;
        }
        .atable .datepicker {
          width:80%;
        }
        .oddRow{
	        background:#FFFFFF;
        }
        .evenRow{
	        background:#DFDFFF;
        }
        .overRow{
	        /*background-color: #0099CC;*/
	        
	        background :transparent url('../../content/images/HeaderFundPick.png') repeat-x center;
	        /*color:#FFFFFF;*/		        
	        border-right: 1px solid #666666;
	        border-bottom: 1px solid #666666;
	        border-top: 1px solid #DDDDDD;	        
        }
        
        
		
        
        /*select with custom icons*/
		body a.customicons { height: 2.8em;}
		body .customicons li a, body a.customicons span.ui-selectmenu-status { line-height: 2em; padding-left: 30px !important; }
		/*
		body .sourcePF .ui-selectmenu-item-icon { height: 10px; width: 10px; }		
		body .sourceAA .ui-selectmenu-item-icon { height: 8px; width: 8px; }
		body .sourceFP .ui-selectmenu-item-icon { height: 20px; width: 20px; }
		body .sourceWL .ui-selectmenu-item-icon { height: 20px; width: 20px; }
		*/
		body .sourcePF .ui-selectmenu-item-icon { background: url('../../Content/images/portfolio_modifica.png') 0 0 no-repeat; height: 100%; width: 100%;}
		body .sourceAA .ui-selectmenu-item-icon { background: url('../../Content/images/chart-icon.png') 0 0 no-repeat; height: 100%; width: 100%;}
		body .sourceFP .ui-selectmenu-item-icon { background: url('../../Content/images/Fp.png') 0 0 no-repeat; height: 100%; width: 100%; }
		body .sourceWL .ui-selectmenu-item-icon { background: url('../../Content/images/list1_add.png') 0 0 no-repeat;height: 100%; width: 100%;}
	
		
        
     </style>
       <%-- /*
        .oddtr
        {
	        background-color:#EFF1F1;
        }
        .eventr
        {
	        background-color:#F8F8F8;
        }
        .trover
        {
	        background-color: #0099CC;
        }
        .trclick
        {
	        background-color: #00CCCC;
        }*/
       --%>
        
    <script type="text/javascript">
        var debug = true;
        jQuery.extend({
            log: function(msg) {
                if (!msg) return;
                if (debug) {
                    if (window.console) {
                        // Firefox & Google Chrome
                        console.log(msg);
                    }
                    else {
                        // Other browsers
                        $("body").append("<div style=\"width:800px;color:#FFFFFF;background-color:#000000;\">" + msg + "</div>");
                    }
                    return true;
                }
            }
        });               

        $.fn.listHandlers = function(events, outputFunction) {
            return this.each(function(i) {
                var elem = this,
                dEvents = $(this).data('events');
                if (!dEvents) { return; }
                $.each(dEvents, function(name, handler) {
                    if ((new RegExp('^(' + (events === '*' ? '.+' : events.replace(',', '|').replace(/^on/i, '')) + ')$', 'i')).test(name)) {
                        $.each(handler, function(i, handler) {
                            outputFunction(elem, '\n' + i + ': [' + name + '] : ' + handler);
                        });
                    }
                });
            });
        };

        $("document").ready(function() {
            //jQuery.log('autoTable');

            //alert('jquery version ' + $().jquery);

            $TipoPort = ($('#TipoPortafoglio').val() != "ASSETALLOCATION") && ($('#TipoPortafoglio').val() != "ASSETALLOCATION BILANCIATO");

            $('select#OrigineDati').selectmenu({
                style: 'dropdown',
                menuWidth: 200,
                width: 200,
                format: addressFormatting,
                icons: [
					{ find: '.sourceFP' },
					{ find: '.sourceAA' },
					{ find: '.sourcePF' },
					{ find: '.sourceWL' }
				]
            });

            $('select#OrigineDati').bind('change', function(e) {
                //e.preventDefault();
                var odSelect = $('select[id$=OrigineDati] :selected').text();
                CambiaOrigine(odSelect);
            });


            function CambiaOrigine(whatSelect) {

                //$('#ComboWLval').val($('#speedA').val());
                var divFP = $('#divCmdFP');
                var divFP1 = $('#divCmdFP1');
                var divAA = $('#divCmdAA');
                var divAA1 = $('#divCmdAA1');
                var divPF = $('#divCmdPF');
                var divPF1 = $('#divCmdPF1');
                var divWL = $('#divCmdWL');
                var divWL1 = $('#divCmdWL1');

                $("#divOrigineDati").html("");
                if (whatSelect == "Fund Picker") {
                    divFP.css({ display: "block" });
                    divFP1.css({ display: "block" });
                    divAA.css({ display: "none" });
                    divAA1.css({ display: "none" });
                    divPF.css({ display: "none" });
                    divPF1.css({ display: "none" });
                    divWL.css({ display: "none" });
                    divWL1.css({ display: "none" });
                    var jqxhr = $.get('<%= Url.Action("Picking", "Products", new {FromOrigin=true}) %>',
                        function(htmlResult) {

                            $("#divOrigineDati").html(htmlResult);
                        });
                }
                if (whatSelect == "Asset Allocation") {
                    divFP.css({ display: "none" });
                    divFP1.css({ display: "none" });
                    divAA.css({ display: "block" });
                    divAA1.css({ display: "block" });
                    divPF.css({ display: "none" });
                    divPF1.css({ display: "none" });
                    divWL.css({ display: "none" });
                    divWL1.css({ display: "none" });
                    var jqxhr = $.get('<%= Url.Action("FromAA", "Portfolios") %>',
                        function(htmlResult) {
                            $("#divOrigineDati").html(htmlResult);
                        });
                }
                if (whatSelect == "Portfoglio") {
                    divFP.css({ display: "none" });
                    divFP1.css({ display: "none" });
                    divAA.css({ display: "none" });
                    divAA1.css({ display: "none" });
                    divPF.css({ display: "block" });
                    divPF1.css({ display: "block" });
                    divWL.css({ display: "none" });
                    divWL1.css({ display: "none" });
                    var jqxhr = $.get('<%= Url.Action("FromCopia", "Portfolios") %>',
                        function(htmlResult) {
                            $("#divOrigineDati").html(htmlResult);
                        });
                }
                if (whatSelect == "Watch List") {
                    divFP.css({ display: "none" });
                    divFP1.css({ display: "none" });
                    divAA.css({ display: "none" });
                    divAA1.css({ display: "none" });
                    divPF.css({ display: "none" });
                    divPF1.css({ display: "none" });
                    divWL.css({ display: "block" });
                    divWL1.css({ display: "block" });
                    var jqxhr = $.get('<%= Url.Action("FromWatchList", "Tools") %>',
                        function(htmlResult) {
                            //alert(htmlResult);
                            $("#divOrigineDati").html(htmlResult);
                        });
                }
            };

            CambiaOrigine("Fund Picker");


            initTab($("#OriginTabs"), {
                reloadOnChange: true, // se tab ajax è ricaricato ad ogni cambio di tab
                change: function(idx) { // evento cambio tab
                    // console.log(idx);
                }
            });

            var tabs = $("#OriginTabs").find('ul').first().children('li');
            tabs.click(function() {

                var divFP = $('#divCmdFP');
                var divFP1 = $('#divCmdFP1');
                var divAA = $('#divCmdAA');
                var divAA1 = $('#divCmdAA1');
                var divPF1 = $('#divCmdPF1');
                if (TabSelected() == 0) {
                    divFP.css({ display: "block" });
                    divFP1.css({ display: "block" });
                    divAA.css({ display: "none" });
                    divAA1.css({ display: "none" });
                    divPF1.css({ display: "none" });
                }
                else if (TabSelected() == 1) {
                    divFP.css({ display: "none" });
                    divAA.css({ display: "block" });
                    divFP1.css({ display: "none" });
                    divAA1.css({ display: "block" });
                    divPF1.css({ display: "none" });
                }
                else if (TabSelected() == 2) {
                    divFP.css({ display: "none" });
                    divAA.css({ display: "none" });
                    divFP1.css({ display: "none" });
                    divAA1.css({ display: "none" });
                    divPF1.css({ display: "block" });
                }
            });

            //$(".delRow").btnDelRow();
            $(".autoTable").tableAutoAddRow({ autoAddRow: true, inputBoxAutoNumber: true });
            $(".alternativeRow").btnAddRow(
            //{ oddRowCSS: "oddRow", evenRowCSS: "evenRow", displayRowCountTo: "rowCount" },
                {oddRowCSS: "oddRow", evenRowCSS: "evenRow", overRowCSS: "overRow" },
                function(row) {

                    //                    alert('RIGA PRIMA \n' + row.html());
                    //                    row.find(".datepicker").datepicker({
                    //                        showOn: "button",
                    //                        buttonImage: "/content/JqueryUICust/images/calendar.gif",
                    //                        buttonImageOnly: true
                    //                    });

                    //                    $('input', row).val('').
                    //                    filter('.hasDatepicker').removeClass('hasDatepicker').datepicker({
                    //                        showOn: "button",
                    //                        buttonImage: "/content/JqueryUICust/images/calendar.gif",
                    //                        buttonImageOnly: true,
                    //                        dateFormat: "dd/mm/yy"
                    //                    });
                    //if ($('#TipoPortafoglio').val() != "ASSETALLOCATION") {
                    if ($TipoPort) {
                        $('input', row).val('').
                        filter('.hasDatepicker').removeClass('hasDatepicker').attr('id', '').datepicker(
                        {
                            changeMonth: true,
                            changeYear: true,
                            showOn: "button",
                            buttonImage: "/content/JqueryUICust/images/calendar.gif",
                            buttonImageOnly: true,
                            dateFormat: "dd/mm/yy"
                        });

                        //                    alert($('IMG', row).val('').
                        //                    filter('.ui-datepicker-trigger').length);
                        $('IMG', row).val('').
                        filter('.ui-datepicker-trigger').last().remove();
                    }
                    //                    alert('RIGA DOPO \n' + row.html());
                    CambiaData();
                });
            //jQuery.log('alternativeRow');
            $(".delRow").btnDelRow(
            //{ displayRowCountTo: "rowCount" },
                function(row) {
                    CalcolaTotControvalore();
                    CalcolaPercentuali();
                    $('.Percentage').each(function(index) {
                        var NumericValue = parseFloat($(this).val());
                        if (!isNaN(NumericValue))
                            $(this).val(NumericValue.toFixed(2) + '%');
                        CalcolaLiquidita();
                    });
                }
             );
            //jQuery.log('delRow');
            $(".delRow").click(function() {
                btnDelRow();
                AggiornaCampi();
                CalcolaValori();
            });
            if ($TipoPort) {
                $(".datepicker").last().datepicker({
                    changeMonth: true,
                    changeYear: true,
                    showOn: "button",
                    buttonImage: "/content/JqueryUICust/images/calendar.gif",
                    buttonImageOnly: true,
                    dateFormat: "dd/mm/yy"

                });
            }
            //            $(".datepicker").formatDate('dd/mm/yyyy');

            $('#btnAddRow').click(function() {
                if ($TipoPort)
                    $('.Percentage').attr("disabled", true);
                AggiornaCampi();
                CalcolaValori();
            });

            //jQuery.log('AggiornaCampi');
            CalcolaValori();
            AggiornaCampi();


            //            $("#autotable tr")
            //            .mouseover(function() { $(this).addClass("overRow"); })
            //            .mouseout(function() { $(this).removeClass("overRow"); })
            //            .click(function() { $(this).toggleClass("trclick"); })
            //

            $("#ControValoreLiquidita").blur(function() {
                CalcolaTotControvalore();
                CalcolaPercentuali();
            });

            CambiaData();

            $.blockUI({
                message: $('#domMessage'),
                //                                fadeIn: 700,
                //                                fadeOut: 700,
                css: {

                    border: 'none',
                    //border: '3px solid #aaa',
                    padding: '15px',
                    backgroundColor: '#000',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .6,
                    color: '#fff'
                }
            });

            setTimeout($.unblockUI, 2000);

            JsonCalls();

        });

        //a custom format option callback
        var addressFormatting = function(text) {
            var newText = text;
            //array of find replaces
            var findreps = [
				{ find: /^([^\-]+) \- /g, rep: '<span class="ui-selectmenu-item-header">$1</span>' },
				{ find: /([^\|><]+) \| /g, rep: '<span class="ui-selectmenu-item-content">$1</span>' },
				{ find: /([^\|><\(\)]+) (\()/g, rep: '<span class="ui-selectmenu-item-content">$1</span>$2' },
				{ find: /([^\|><\(\)]+)$/g, rep: '<span class="ui-selectmenu-item-content">$1</span>' },
				{ find: /(\([^\|><]+\))$/g, rep: '<span class="ui-selectmenu-item-footer">$1</span>' }
			];

            for (var i in findreps) {
                newText = newText.replace(findreps[i].find, findreps[i].rep);
            }
            return newText;
        };

        CambiaData = function() {
            if ($TipoPort) {
                $('.datepicker').unbind('change');
                $(".datepicker").change(function() {
                    //alert('Cambio data');
                    var cells = this.parentNode.parentNode.cells;
                    var FundID = cells[0].children[1].value;
                    var datep=this.value;
                    JsonGetPrize(FundID,datep, this);
                });
            }
        };

        JsonGetPrize = function(FundID, datep, obgDP) {
            //jQuery.log('Json call');
            var url = "/Portfolios/GetFundPrize";
            var PortfoglioID = $('#PortfoglioID').val();
            var UserID = $('#UserID').val();
            var noCache = new Date().getTime();
            $.getJSON(url,
                { id: PortfoglioID, FundID: FundID, noCache: datep + '_' + noCache, gUserID: UserID },
                function(data) {
                    obgDP.value = data.unitPriceDate;
                    var cells = obgDP.parentNode.parentNode.cells;
                    cells[4].children[0].value = data.unitPrice.toFixed(2);
                    
                });
        };

        JsonCalls = function() {
            //jQuery.log('Json call');
            var url = "/Portfolios/GetOperationPortfoglio";
            var PortfoglioID = $('#PortfoglioID').val();
            var UserID = $('#UserID').val();

            //alert('JSON Call');
            //$.ajaxSetup({ cache: false });


            var FromPB = $('#PortfolioBenchmark');
            //se arriva direttamente da PortfolioBenchmark
            //nn ha bisogno di tirarsi su i dati ma li mette direttamente
            if (FromPB.length > 0) {
                //alert(FromPB.val());
                $($("#OriginTabs > ul > li")[1]).find('a').click();

                $('#TipoAA').val(FromPB.val());
                $('#TipoAA').trigger('change');

                AggiungiPFBenchmark();
                $('div.blockMe').unblock();
            }
            else {

                var noCache = new Date().getTime();
                $.getJSON(url,
                    { id: PortfoglioID, gUserID: UserID + '_' + noCache },
                    function(data) {
                        if (data.length > 0) {
                            //$('#DataContainer').html('data.length  ' + data.length + ' ' + data[0]._fidaCode + ' ' + data[0]._operationDate);
                            for (i = 0; i < data.length; i++) {
                                //alert(i + ' ' + data[i]._fundName);
                                AggiungiRiga({ Id: data[i]._fidaCode,
                                    Desc: data[i]._fundName,
                                    opDate: data[i]._operationDate,
                                    weight: data[i]._weight,
                                    price: data[i]._price,
                                    quantity: data[i]._quantity,
                                    unitPrice: data[i]._unitPrice
                                });
                                //alert(i);
                                $('.Percentage').each(function(index) {
                                    var NumericValue = parseFloat($(this).val());
                                    if (!isNaN(NumericValue))
                                        $(this).val(NumericValue.toFixed(2) + '%');
                                    CalcolaLiquidita();
                                });
                                AggiornaCampi();
                                CalcolaValori();
                                CalcolaTotControvalore();
                                CalcolaPercentuali();
                            }
                            $('div.blockMe').unblock();
                        }
                    });
            }
        };


        AggiornaDate = function() {
            //jQuery.log('Definizione datepicker');
            var DateP = $("#datepicker");
            if (DateP != null)
                DateP.datepicker({
                    changeMonth: true,
                    changeYear: true,
                    showOn: "button",
                    buttonImage: "/content/JqueryUICust/images/calendar.gif",
                    buttonImageOnly: true
                    
                });
            };

            PulisciPF = function() {
                $TableDest = $('#' + 'autotable');
                var rows = $TableDest.find('tbody > tr').get();
                $cnt = rows.length
                $i = $cnt - 1;

                while ($i > 2) {
                    $TableDest.children().children().last().remove();
                    $i--;
                }
                var FieldsTD = $TableDest.children().children().last().find('td').get();
                $cnt = FieldsTD.length
                $i = 0;

                while ($i < $cnt) {
                    var campo = FieldsTD[$i].children[0];
                    campo.value = "";
                    if ($i == 0) {
                        var campo = FieldsTD[$i].children[1];
                        campo.value = "";
                    }
                    $i++;
                }
                CalcolaLiquidita();
                CalcolaTotControvalore();
                CalcolaPercentuali();


            };

            ControlloDati = function() {
                var ret = true;
                var Quanti = 0;
                ret = ret && ControlloLiquidita();
                if ($TipoPort) {

                    $('.NumQuote').each(function() {
                        var cicloVal = parseFloat($(this).val());
                        if (!isNaN(cicloVal)) {
                            Quanti++;
                        }
                    })
                    
                    if ((Quanti == 0) )
                        alert('Attenzione \n salverai un portfoglio con la sola informazione di liquidità');                    
                }
                else {

                    $('.Percentage').each(function() {
                        var cicloVal = parseFloat($(this).val());
                        if (!isNaN(cicloVal)) {
                            Quanti++;
                        }
                    })
                    
                    if ((Quanti == 0) || (Quanti == 1))
                        alert('Attenzione \n salverai un portfoglio con la sola informazione di liquidità');

                }

                return ret;
            };
        
        
        
        ControlloLiquidita = function() {
            var Liquidita = $('.Percentage').last();

            if (Liquidita.val() < 0) {
                alert('Attenzione \n non valido il valore di liquidità');
                return false;
            }
            else
                return true;
        };

        disableEnterKey = function(e) {
            var key;
            if (window.event)
                key = window.event.keyCode;     //IE
            else
                key = e.which;     //firefox
            if (key == 13)
                return false;
            else
                return true;
        };
        CalcolaTotQuote = function() {
            var TotQuote = $('.TotQuote');
            var Tot = 0;
            $('.NumQuote').each(function(index) {
                var cicloVal = parseFloat($(this).val());
                if (!isNaN(cicloVal)) {
                    Tot = Tot + cicloVal;
                }
            });
            TotQuote.val(Tot);
        };
        CalcolaTotControvalore = function() {
            var TotControValore = $('.TotControValore');
            var Tot = 0;
            $('.ControValore').each(function(index) {
                var cicloVal = parseFloat($(this).val());
                if (!isNaN(cicloVal)) {
                    Tot = Tot + cicloVal;
                }
            });
            var cicloVal = parseFloat($('#ControValoreLiquidita').val());
            if (!isNaN(cicloVal)) {
                Tot = Tot + cicloVal;
            }
            TotControValore.val(Tot);
        };
        CalcolaPercentuali = function() {

            if ($TipoPort) {
                $('.Percentage').each(function(index) {
                    var ChildrenRow = $(this).parent().parent().children();
                    var cellControvalore = ChildrenRow.find('.ControValore').get();
                    if (cellControvalore.length==0)
                        var cellControvalore = ChildrenRow.find('.ControValoreLiquidita').get();
                   
                    var nvControvalore = parseFloat(cellControvalore[0].value);
                    
                    var nvControvaloreTot = parseFloat($('.TotControValore').val())
                    var Perc = nvControvalore / nvControvaloreTot * 100;
                    if (isNaN(Perc))
                        Perc = 0;
//                    var cellPerc = ChildrenRow.find('.Percentage').get();
//                    cellPerc[0].value = Perc;
                    $(this).val(Perc.toFixed(2) + '%');
                })
            };
        };
        CalcolaValori = function() {
            if ($TipoPort) {
                $('.NumQuote').unbind('blur');
                $('.Valore').unbind('blur');
                $('.ControValore').unbind('blur');
                $('.NumQuote').blur(function() {

                    var nvQuote = parseFloat($(this).val());
                    if (isNaN(nvQuote))
                        nvQuote = 0;
                    var ChildrenRow = $(this).parent().parent().children();
                    var cellValore = ChildrenRow.find('.Valore').get();
                    var nvValore = parseFloat(cellValore[0].value);
                    if (isNaN(nvValore))
                        nvValore = 0;
                    var cellControvalore = ChildrenRow.find('.ControValore').get();
                    var nvControvalore = parseFloat(cellControvalore[0].value);
                    if (isNaN(nvControvalore))
                        nvControvalore = 0;

                    if (nvValore != 0)
                    //cellControvalore.val(nvQuote * nvValore);
                        cellControvalore[0].value = nvQuote * nvValore;
                    else
                        if (nvQuote != 0)
                        cellValore[0].value = nvControvalore / nvQuote;

                    //CalcolaTotQuote();
                    CalcolaTotControvalore();
                    CalcolaPercentuali();


                });
                $('.Valore').blur(function() {
                    var nvValore = parseFloat($(this).val());
                    if (isNaN(nvValore))
                        nvValore = 0;
                    var ChildrenRow = $(this).parent().parent().children();
                    var cellQuote = ChildrenRow.find('.NumQuote').get();
                    var nvQuote = parseFloat(cellQuote[0].value);
                    if (isNaN(nvQuote))
                        nvQuote = 0;
                    var cellControvalore = ChildrenRow.find('.ControValore').get();
                    var nvControvalore = parseFloat(cellControvalore[0].value);
                    if (isNaN(nvControvalore))
                        nvControvalore = 0;
                    //cellControvalore.val(nvQuote * nvValore);
                    if (nvQuote != 0)
                        cellControvalore[0].value = nvQuote * nvValore;
                    else
                        if (nvValore != 0)
                        cellQuote[0].value = nvControvalore / nvValore;

                    //CalcolaTotQuote();
                    CalcolaTotControvalore();
                    CalcolaPercentuali();

                });
                $('.ControValore').blur(function() {
                    var nvControvalore = parseFloat($(this).val());
                    if (isNaN(nvControvalore))
                        nvControvalore = 0;
                    var ChildrenRow = $(this).parent().parent().children();
                    var cellQuote = ChildrenRow.find('.NumQuote').get();
                    var nvQuote = parseFloat(cellQuote[0].value);
                    if (isNaN(nvQuote))
                        nvQuote = 0;
                    var cellValore = ChildrenRow.find('.Valore').get();
                    var nvValore = parseFloat(cellValore[0].value);
                    if (isNaN(nvValore))
                        nvValore = 0;

                    if (nvValore != 0)
                        cellQuote[0].value = nvControvalore / nvValore;
                    else
                        if (nvQuote != 0)
                        cellValore[0].value = nvControvalore / nvQuote;

                    //CalcolaTotQuote();
                    CalcolaTotControvalore();
                    CalcolaPercentuali();

                });


            }
        };
        CalcolaLiquidita = function() {
            //jQuery.log('Ciclo per calcolo Tot');
            var Liquidita = $('.Percentage').last();
            var Tot = 0;
            $('.Percentage').each(function(index) {
                if (index < $('.Percentage').size() - 1) {
                    //alert($('.Percentage').size())
                    //alert(index);
                    //alert($(this).val());
                    var cicloVal = parseFloat($(this).val());
                    //alert('cicloVal ' + cicloVal);
                    if (!isNaN(cicloVal)) {
                        if ($(this).attr('id') != 'IdLiquidita') {
                            //alert(index + ' : ' + $(this).val() + ' id: ' + $(this).attr('id'));
                            Tot = Tot + cicloVal;
                            //alert('Tot ' + Tot);
                        }
                    }
                }
            });
            Liquidita.val(100 - Tot);
        };

        AggiornaCampi = function() {
        //jQuery.log('Percentage blur');
            $('.Percentage').unbind('blur');
            $('.Percentage').blur(function() {
                var NumericValue = parseFloat($(this).val());
                if (!isNaN(NumericValue))
                    $(this).val(NumericValue.toFixed(2) + '%');
                CalcolaLiquidita();
            });
        };
        
        AggiungiPFBenchmark = function() {

            var checkSelected = $(":checkbox");
            PulisciPF();
            var divAA = $(".divAA")[$('#TipoAA').val() - 1];
            var rows = divAA.children[0].children[0].rows;

            for (x = 1; x < rows.length; x += 2) {
                var _weight = rows[x].children[2].innerHTML;
                var DescFondo = rows[x + 1].children[0].childNodes[0].nodeValue;
                var IdFondo = rows[x + 1].children[0].childNodes[1].value;
                //                alert(IdFondo);
                //                alert(DescFondo);
                //                alert(_weight);
                AggiungiRiga({ Id: IdFondo, Desc: DescFondo, weight: _weight });
            };
            
            if ($TipoPort) {
                CalcolaTotControvalore();
                CalcolaPercentuali();
            }
            else {                
                CalcolaLiquidita();
            }

        };

        TabSelected = function() {
            var tabs = $("#OriginTabs").find('ul').first().children('li');
            var TabSel = 0;
            tabs.each(function(idx) {
                var tab = $(this);
                if (tab.hasClass('selected'))
                    TabSel = idx;
            });
            return TabSel;
        };
        TransferElementsPF = function() {

            var HiddenSelectedNodes = $('#HiddenSelectedNodes').val();
            if (HiddenSelectedNodes.length > 0) {
                var SelectedNodes = HiddenSelectedNodes.split('},');
                //alert(SelectedNodes);
                $.map(SelectedNodes, function(node) {
                    //alert(node.data.key);
                    if (node.substring(node.length - 1) != '}')
                        node = node + '}';
                    var strsx = Left(node, node.indexOf(","));
                    var rxnode = Right(node, node.length - strsx.length - 1);
                    strsx = Left(strsx, 4) + "'" + Right(strsx, strsx.length - 4) + "',";

                    var strsx1 = Left(rxnode, rxnode.indexOf(","));
                    var rxnode1 = Right(rxnode, rxnode.length - strsx1.length - 1);
                    strsx1 = Left(strsx1, strsx1.indexOf(":") + 1) + "'" + Right(strsx1, strsx1.length - strsx1.indexOf(":") - 1) + "',";

                    node = strsx + strsx1 + rxnode1;
                    //alert(node);
                    var objNode = eval("(" + node + ")");
                    if (objNode.Id != "XXLIQ") {
                        AggiungiRiga(objNode);
                    }
                });

                if ($TipoPort) {
                    CalcolaTotControvalore();
                    CalcolaPercentuali();
                }
                else {
                    CalcolaLiquidita();
                }
            }

        };
        TransferElements = function() {

            $TableDest = $('#autotable');
            var checkSelected = $(":checkbox:checked");
            if (checkSelected.length == 0)
                alert('Attenzione ! \nNon hai selezionato elementi da trasferire');
            var cs = $("input:checkbox").attr("checked");

            checkSelected.each(function(index) {
                var IdFondo = $(this).attr("value");
                var row = $(this).parent().parent();
                var DescFondo = row.children().first().next().text();

                AggiungiRiga({ Id: IdFondo, Desc: DescFondo });
            });
        };

        TransferElementsWL = function() {

            var HiddenSelectedNodes = $('#HiddenSelectedNodes').val();
            if (HiddenSelectedNodes.length > 0) {
                var SelectedNodes = HiddenSelectedNodes.split('},');
                //alert(SelectedNodes);
                $.map(SelectedNodes, function(node) {
                    //alert(node.data.key);
                    if (node.substring(node.length - 1) != '}')
                        node = node + '}';
                    //alert(node);
                    var strsx = Left(node, node.indexOf(","));
                    var rxnode = Right(node, node.length - strsx.length - 1);
                    strsx = Left(strsx, 4) + "'" + Right(strsx, strsx.length - 4) + "',";

                    var rxnode1 = Right(rxnode, rxnode.length - rxnode.indexOf(":") - 1);
                    rxnode1 = Left(rxnode1, rxnode1.length - 1);

                    node = strsx + Left(rxnode, 5) + "'" + rxnode1 + "'}";
                    alert(node);
                    var objNode = eval("(" + node + ")");
                    AggiungiRiga(objNode);
                });
            }
        };
        



        AggiungiRiga = function() {
            var args = arguments[0] || {};
            var Id = args.Id;
            var Desc = args.Desc;
            var opDate = args.opDate;
            var weight = args.weight;

            var price = args.price;
            var quantity = args.quantity;
            var unitPrice = args.unitPrice;

            if (typeof quantity == "undefined")
                quantity = 0;
            if (typeof price == "undefined")
                price = 0;
            if (typeof unitPrice == "undefined")
                unitPrice = 0;

            

            $TableDest = $('#' + 'autotable');
            var rows = $TableDest.find('tbody > tr').get();
            var FieldsTD = $TableDest.find('tbody > tr > td').get();

            var $Ids = $TableDest.find('tbody > tr > td > input#IdFondo').get();



            //alert(TipoP);

            if (!PresenteValore($Ids, Id)) {

                var outptut = '';

                var LastID = $TableDest.find('tbody > tr > td > input#IdFondo').last();
                if (LastID[0].value != "")
                    if (Id != "XXLIQ")
                    $('#btnAddRow').click();

                $('.delRow').click(function() {
                    AggiornaCampi();
                    CalcolaTotControvalore();
                    CalcolaPercentuali();
                });


                //jQuery.log('Dopo');
                //alert($TableDest.children().children().length);
                //var LastRow = $TableDest.children().children().last().prev();
                var LastRow = $TableDest.children().children().last();


                if (Id != "XXLIQ") {
                    var cell = LastRow[0].children[0];
                    cell.children[0].value = Desc;
                    cell.children[1].value = Id;
                    if ($TipoPort) {
                        cell = LastRow[0].children[3];
                        cell.children[0].value = quantity;
                        cell = LastRow[0].children[4];
                        cell.children[0].value = unitPrice;
                        cell = LastRow[0].children[5];
                        cell.children[0].value = price;

                    }
                    if (weight) {
                        cell = LastRow[0].children[1];
                        cell.children[0].value = weight;
                    }
                    else {
                        cell = LastRow[0].children[1];
                        cell.children[0].value = 0;
                    }
                    if ($TipoPort) {
                        if (opDate) {
                            cell = LastRow[0].children[2];
                            cell.children[0].value = opDate;
                        }
                        else {
                            cell = LastRow[0].children[2];
                            var date_obj = new Date();

                            var d = date_obj.getDate();
                            var m = date_obj.getMonth();
                            var y = date_obj.getFullYear();

                            var df = (date_obj.getDate()) + '/' + (date_obj.getMonth() + 1) + '/' + date_obj.getFullYear();
                            //                    alert(df);
                            cell.children[0].value = df;
                        };
                    }
                    //$TableDest.children().children().last().remove();
                    //$('.delRow').last().btnDelRow();

                }

                else {
                    $TableDest = $('#TableLiquidita');
                    if (!$TipoPort) {
                        var LastRow = $TableDest.children().children().last();
                    }
                    else {
                        var LastRow = $TableDest.children().children().last().prev();
                    }
                    var cells = LastRow[0].children[0];
                    cells.children[0].value = Desc;
                    cells.children[1].value = Id;
                    cells = LastRow[0];
                    if ($TipoPort)
                        cells.children[5].children[0].value = price;
                    else
                        cells.children[1].value = weight;

                }
                AggiornaCampi();
                CalcolaValori();
            }
            else
            { alert('Elemento già inserito'); }

        };

            PresenteValore = function(obj, value) {
                var presente = false;
                $.each(obj, function() {
                    var elem = $(this);
                    if (elem.val() == value)
                    { presente = true; }
                });
                return presente;
            };
            precedente = function() {
                $('#btnPrecedente').click();
            }
    </script>

    <%--<a href="javascript:switchViews('divProva', 'one');">
        <img id="imgdivProva" alt="Apri/Chiudi gli elementi" border="0" src="/Content/images/dwn.gif" />
    </a>
        <div id="divProva" style="border:1px solid #507CD1; display:none;position:relative;left:25px;width:97%"  >
            
        </div>--%>
        
     <div id="DataContainer">
         
     </div>
     
     <div id="domMessage" style="display:none;">
    
        <img src="<%= Url.Content("~/Content/images/LogoH.png") %>" 
	     style="width:100px;position:relative;left:-10;top:-10" /> 
    
    
        <p><img src="<%=Url.Content("~/Content/images/busy.gif")%>" /> Attendere prego ...</p>
     </div>
        
     <% 
         
         if (ViewData.Count == 0)
         {
            %>
            Modello vuoto
            probabile problema di Timeout              
            <%
         }
         else
         {%>
         <div >
           
         <%
         string TipoPortfoglio=ViewData["TipoPortfoglio"].ToString();

         using (Html.BeginForm("Struttura", "Portfolios", new { id = Convert.ToInt32(ViewData["PortfoglioID"]) }, FormMethod.Post))
         { 
             %>  
           
            <%
             if (ViewData.ContainsKey("PortfolioBenchmark"))
            {%>
            <%= Html.Hidden("PortfolioBenchmark", ViewData["PortfolioBenchmark"])%>
            <%} %>
            <%= Html.Hidden("TipoPortafoglio", ViewData["TipoPortfoglio"])%>  
            <%= Html.Hidden("UserID", ViewData["UserID"])%>  
            <%= Html.Hidden("PortfoglioID", ViewData["PortfoglioID"])%>  
            <%= Html.Hidden("CurrentUICulture", System.Threading.Thread.CurrentThread.CurrentUICulture.ToString())%> 
            
        <div class="blockMe">       
            
            <table style="width:100%">                                 
                <tr>
                    <td style="width:40%; vertical-align:top;">                        
                        <div style="margin-top:5px;" class="boxSrcHd">
                            <table style="width:100%">
                                <tr>
                                    <td>Origine Dati</td>
                                    <td>
                                        <div style="padding: 0px 0px 0px 0px;text-align:right;display:block;" id="divCmdFP1">
                                            <a href="javascript:TransferElements();">
                                               <%-- <img id="imgBtnAddPF" alt="aggiungi selezionati" border="0"  src="<%= Url.Content("~/Content/images/next.gif")%>" />--%>
                                                Aggiungi elementi
                                            </a>                                            
                                        </div>
                                        <div style="padding: 0px 0px 0px 0px;text-align:right;display:none;" id="divCmdAA1">
                                            <a href="javascript:AggiungiPFBenchmark();">
                                               <%-- <img id="imgBtnAddPF" alt="aggiungi selezionati" border="0"  src="<%= Url.Content("~/Content/images/next.gif")%>" />--%>
                                                Aggiungi portfoglio benchmark
                                            </a>
                                        </div>
                                        <div style="padding: 0px 0px 0px 0px;text-align:right;display:none;" id="divCmdPF1">
                                            <a href="javascript:TransferElementsPF();">
                                               <%-- <img id="imgBtnAddPF" alt="aggiungi selezionati" border="0"  src="<%= Url.Content("~/Content/images/next.gif")%>" />--%>
                                                Aggiungi elementi
                                            </a>
                                        </div>
                                        <div style="padding: 0px 0px 0px 0px;text-align:right;display:none;" id="divCmdWL1">
                                            <a href="javascript:TransferElementsWL();">
                                               <%-- <img id="imgBtnAddPF" alt="aggiungi selezionati" border="0"  src="<%= Url.Content("~/Content/images/next.gif")%>" />--%>
                                                Aggiungi elementi
                                            </a>
                                        </div>
                                    </td>                                    
                                </tr>                                
                            </table>                            
                        </div>
                        <div class="boxSrcCn" id="Div1" style="padding:5px 10px;"> 
                            <%--<div class="resBox">                 
                                <div id="OriginTabs">
                                    <ul class="resTab">   
                                        <li class="rnd5Top">
                                        <a href="<%=  Url.Action("Picking", "Products", new {FromOrigin=true})%>">FundPicker</a></li>                                       
                                        <li class="rnd5Top">
                                        <a href="<%= Url.Action("FromAA") %>">AssetAllocation</a></li>                                        
                                        <li class="rnd5Top">
                                        <a href="<%= Url.Action("FromCopia") %>">Portfoglio</a></li>
                                    </ul>                                    
                                    <div class="resInBox">
                                        <div class="clear"></div>
                                    </div>
                        	        <div class="resInBox">
                                        <div class="clear"></div>
                                    </div>
                                    <div class="resInBox">
                                        <div class="clear"></div>
                                    </div>                 
                                </div>
                            </div> --%>  
                            
                            <select name="OrigineDati" id="OrigineDati" class="customicons">
                                <option value="FP" class="sourceFP">Fund Picker</option>
                                <option value="AA" class="sourceAA">Asset Allocation</option>
                                <option value="PF" class="sourcePF">Portfoglio</option>
                                <option value="WL" class="sourceWL">Watch List</option>
                            </select>    
                            <div id="divOrigineDati">
                            
                            </div>
                            
                            <div style="padding: 20px 40px 0 0;text-align:right;display:block" id="divCmdFP">
                                <a href="javascript:TransferElements();">
                                   <%-- <img id="imgBtnAddPF" alt="aggiungi selezionati" border="0"  src="<%= Url.Content("~/Content/images/next.gif")%>" />--%>
                                    Aggiungi elementi
                                </a>                                
                            </div>
                            <div style="padding: 20px 40px 0 0;text-align:right;display:none" id="divCmdAA">
                                <a href="javascript:AggiungiPFBenchmark();">
                                   <%-- <img id="imgBtnAddPF" alt="aggiungi selezionati" border="0"  src="<%= Url.Content("~/Content/images/next.gif")%>" />--%>
                                    Aggiungi portfoglio benchmark
                                </a>
                            </div>   
                            <div style="padding: 0px 0px 0px 0px;text-align:right;display:none;" id="divCmdPF">
                                <a href="javascript:TransferElementsPF();">
                                   <%-- <img id="imgBtnAddPF" alt="aggiungi selezionati" border="0"  src="<%= Url.Content("~/Content/images/next.gif")%>" />--%>
                                    Aggiungi elementi
                                </a>
                            </div>
                            <div style="padding: 0px 0px 0px 0px;text-align:right;display:none;" id="divCmdWL">
                                <a href="javascript:TransferElementsWL();">
                                   <%-- <img id="imgBtnAddPF" alt="aggiungi selezionati" border="0"  src="<%= Url.Content("~/Content/images/next.gif")%>" />--%>
                                    Aggiungi elementi
                                </a>
                            </div>          
                        </div>                    
                        <%--        <div style="margin-top:5px;" class="boxSrcHd">Fund Picker</div>
                        <div class="boxSrcCn" style="padding:5px 10px;">   
                            <% 
                    
         IEnumerable<GroupResult<FundSelectorDTO>> FundPicking =
             (IEnumerable<GroupResult<FundSelectorDTO>>)ViewData["FundPicking"];
         if (FundPicking != null)
         {
             ViewDataDictionary ViewDataPV = new ViewDataDictionary();
             ViewDataPV["OptionalFromStruttura"] = "YES";
             foreach (GroupResult<FundSelectorDTO> g in FundPicking)
             {
                 Html.RenderPartial("~/Views/Products/FundPickByCategory.ascx", g, ViewDataPV);
             }
         }
         else
         {
             Response.Write("FundPicking Vuoto"); 
                                    
                                    %>
                                    <a href="<%= Url.Action("Struttura", "Portfolios", 
                                                    new { 
                                                        id = Convert.ToInt32(ViewData["PortfoglioID"]), 
                                                        gUserID =  ViewData["UserID"].ToString(), 
                                                        TipoP="ASSETALLOCATION"}
                                                     ) 
                                             %>">
                                    ricarica fund picker</a>
                                    
                                    <%
         }--%>                            
                    </td>                    
                    <%
                        System.Web.HttpBrowserCapabilities browser = Request.Browser;
                        string s = "Browser Capabilities\n"
                            + "Type = " + browser.Type + "\n"
                            + "Name = " + browser.Browser + "\n"
                            + "Version = " + browser.Version + "\n"
                            + "Major Version = " + browser.MajorVersion + "\n"
                            + "Minor Version = " + browser.MinorVersion + "\n"
                            + "Platform = " + browser.Platform + "\n"
                            + "Is Beta = " + browser.Beta + "\n"
                            + "Is Crawler = " + browser.Crawler + "\n"
                            + "Is AOL = " + browser.AOL + "\n"
                            + "Is Win16 = " + browser.Win16 + "\n"
                            + "Is Win32 = " + browser.Win32 + "\n"
                            + "Supports Frames = " + browser.Frames + "\n"
                            + "Supports Tables = " + browser.Tables + "\n"
                            + "Supports Cookies = " + browser.Cookies + "\n"
                            + "Supports VBScript = " + browser.VBScript + "\n"
                            + "Supports JavaScript = " +
                                browser.EcmaScriptVersion.ToString() + "\n"
                            + "Supports Java Applets = " + browser.JavaApplets + "\n"
                            + "Supports ActiveX Controls = " + browser.ActiveXControls
                                  + "\n"
                            + "Supports JavaScript Version = " +
                                browser["JavaScriptVersion"] + "\n";
                        
                        string brWidth = "width:184px;";
                        string LiqWidth = "border:0px;width:415px;";
                        if (browser.Browser == "IE")
                        {
                            brWidth = "width:182px;";
                            LiqWidth = "border:0px;width:425px;";
                        }                        
                    %>                    
                    <td style="width:60%; vertical-align:top;">
                        <div style="margin-top:5px;" class="boxSrcHd">Composizione</div>
                        <div class="boxSrcCn" style="padding:5px 10px;">
                        
                            <%= Html.ValidationSummary(true)%>
                            
                            <%
                                string widthAutoTable = string.Empty;
             
                                if ((TipoPortfoglio == "ASSETALLOCATION") || 
                                (TipoPortfoglio == "ASSETALLOCATION BILANCIATO"))
                                    widthAutoTable = "width:100%";
                                else
                                    widthAutoTable = "width:0px";
                            %>
                                <table id="autotable"  border="1" class="atable dat" style=<%=widthAutoTable%>>                            
                                <tr>
                                    <% 
                                     string ColSpan = "";
                                     if ((TipoPortfoglio == "ASSETALLOCATION") || 
                                         (TipoPortfoglio == "ASSETALLOCATION BILANCIATO"))
                                         ColSpan = "4";
                                     else
                                         ColSpan = "7";
                                            
                                    %>
                                    <th class ="portfoglio" colspan="<%=ColSpan %>">Inserisci i tuoi dati
                                        <input type="button" value="Add Row" id="btnAddRow" class="alternativeRow" style="visibility:hidden"/>
                                    </th>
                                    
                                </tr>
                            <%--<tbody>--%>
                                <tr>            
                                    <% 
                                     if ((TipoPortfoglio == "ASSETALLOCATION") || 
                                         (TipoPortfoglio == "ASSETALLOCATION BILANCIATO"))
                                     {
                                    %>         
                                        <th>Fondo</th>                     
                                        <th>Perc.</th>   
                                    <%
                                     }
                                     else
                                     {
                                    %> 
                                        <th>Fondo</th>                     
                                        <th>Perc.</th>                     
                                        <th>Data</th>   
                                        <th>num Quote</th>
                                        <th>Valore</th>
                                        <th>Contro Valore</th>
                                    <%
                                    }
                                    %>
                                                    
                                </tr>
                                <tr style="height:20px"> 
                                    <% 
                                     if ((TipoPortfoglio == "ASSETALLOCATION") || 
                                     (TipoPortfoglio == "ASSETALLOCATION BILANCIATO"))
                                     {
                                    %>                                                                              
                                        <td style="width:400px">                                        
                                            <input name="NomeFondo" readonly="readonly" style="border:0px;width:400px">
                                            <input id="IdFondo" name="IdFondo" type="hidden" value=""/>
                                        </td>                     
                                        <td style="width:60px">  
                                            <input type="text" id="PercentageField" name="PercentageField" class='Percentage' value="" style="width:50px" onKeyPress="return disableEnterKey(event)"/>                                        </td>
                                    <%
                                     }
                                     else
                                     {
                                    %> 
                                        <td style=<%="'"+brWidth+"'"%>>
                                        <%--<%="<td style='"+brWidth+"' >"%>  --%>                                     
                                            <%--<input name="NomeFondo" readonly="readonly" style="border:0px;width:200px">--%>
                                            
                                            <textarea name="NomeFondo" readonly="readonly" style="border:0px;width:180px;overflow:hidden;font-family: sans-serif;font-size: 1.1em;" tabindex="0"></textarea>
                                            <input id="IdFondo" name="IdFondo" type="hidden" value=""/>
                                        </td>                     
                                        <td>  
                                            <input type="text" id="PercentageField" name="PercentageField" class="Percentage" value="" style="border:0px;width:50px" onKeyPress="return disableEnterKey(event)"/>                                        </td>
                                        <td style="width:90px">
                                            <%            
                                             DateTime dt = DateTime.Now;
                                             string strDate = String.Format("{0:d}", dt);
                                            %>
                                            <input type="text" style="width:65px" id="datepicker" class="datepicker" name="StartDate" value= "<%= strDate %>"/>                                    
                                            <input readonly="readonly" value="" style="border:0px;width:90px;height:0px" onKeyPress="return disableEnterKey(event)">                                                         
                                        </td>   
                                        <td>     
                                            <input type="text" id="NumQuote" name="NumQuote" class='NumQuote' value="" style="width:40px" onKeyPress="return disableEnterKey(event)"/>                                            
                                        </td>                                 
                                        <td>
                                            <input type="text" id="Valore" name="Valore" class='Valore' value="" style="width:40px" onKeyPress="return disableEnterKey(event)"/>
                                        </td> 
                                        <td>
                                            <input type="text" id="ControValore" name="ControValore" class='ControValore' value="" style="width:60px;" onKeyPress="return disableEnterKey(event)"/>
                                        </td> 
                                    <% 
                                    }
                                    %>                                    
                                   
                                        <td style="width:30px">                                    
                                            <img src="<%=Url.Content("~/Content/images/X.png")%>" class="delRow" border="0" style="width:15px">                                        
                                        </td>                                                
                                </tr>
                                <%--</tbody>--%>
                                </table>                                                        
                                <table id="TableLiquidita"  border="1" class="atable dat" style="width:0px">
                                                   
                                    <tr>                                                                              
                                        <% 
                                         if ((TipoPortfoglio == "ASSETALLOCATION") || 
                                         (TipoPortfoglio == "ASSETALLOCATION BILANCIATO"))
                                         {
                                        %>
                                            <td >                                        
                                                <%=" <input name='NomeFondo' readonly='readonly' style='"+LiqWidth+"' >"%> 
                                                <input id="HiddenFondo" name="IdFondoLiquidita" type="hidden" value="IdFondoLiquidita"/>
                                            </td>   
                                            <td style="width:62px">  
                                                <input type="text" name="Liquidita" value="" class='Percentage'  id="IdLiquidita"   style="width:52px" />             
                                            </td>  
                                        <%
                                         }
                                         else
                                         {
                                        %>                                             
                                            <td style=<%="'"+brWidth+"'"%>>
                                            <%--<%="<td style='"+brWidth+"' >"%> --%>                                     
                                                <input name="NomeFondo" readonly="readonly" style="border:0px;width:180px" tabindex="0">
                                                <input id="HiddenFondo" name="IdFondoLiquidita" type="hidden" value="IdFondoLiquidita"/>
                                            </td>   
                                            <td style="width:40px">  
                                                <input type="text" name="Liquidita" value="" class='Percentage'  id="IdLiquidita"  readonly="readonly" style="border:0px;width:50px" />                         
                                            </td> 
                                            <td style="width:90px">
                                                <input readonly="readonly" value="" style="border:0px;width:90px;height:0px" onKeyPress="return disableEnterKey(event)">
                                            </td> 
                                            <td>
                                                <input readonly="readonly" value="" style="border:0px;width:43px;height:0px" onKeyPress="return disableEnterKey(event)"> 
                                            </td> 
                                            <td style="width:40px">
                                                <input type="text" name="" value=""   id="Text3"  readonly="readonly" style="border:0px;width:40px;" /> 
                                            </td> 
                                            <td >
                                                <input type="text" id="ControValoreLiquidita" name="ControValoreLiquidita" class='ControValoreLiquidita'   style="width:60px;" onKeyPress="return disableEnterKey(event)" /> 
                                            </td> 
                                        <%} %>                    
                                    </tr>
                                    <% 
                                     if ((TipoPortfoglio == "ASSETALLOCATION") || (TipoPortfoglio == "ASSETALLOCATION BILANCIATO"))
                                     {
                                    %>
                                    
                                    <%
                                     }
                                     else
                                     {
                                    %> 
                                    <tr>
                                        <%--<td style="width:184px;">  --%>
                                        <td style=<%="'"+brWidth+"'"%>  >                                     
                                            <input readonly="readonly" value="Totali" style="border:0px;width:180px" onKeyPress="return disableEnterKey(event)">                        
                                        </td>   
                                        <td style="width:50px">                          
                                        </td> 
                                        <td style="width:90px">
                                            <input readonly="readonly" value="" style="border:0px;width:90px;height:0px" onKeyPress="return disableEnterKey(event)">
                                        </td> 
                                        <td>
                                            <input readonly="readonly" value="" style="border:0px;width:43px;height:0px" onKeyPress="return disableEnterKey(event)">
                                        </td> 
                                        <td style="width:40px">                        
                                        </td> 
                                        <td >
                                            <input type="text" name="TotControValore" value=""   class='TotControValore' id="TotControValore"  readonly="readonly" style="border:0px;width:60px" onKeyPress="return disableEnterKey(event)"/> 
                                        </td> 
                                    </tr>
                                    
                                    <%} %>
                                </table>                           
                                <table style="width:100%">
                                    <tr>
                                        <td style="width:50%">
                                            <a href="javascript:void(0)" onclick="PulisciPF();return false;">Elimina righe</a>
                                        </td>
                                        <td style="width:30%; text-align:right;">
                                            <a href="#" onclick="precedente()">Precedente</a>                                            
                                        </td>
                                        <td style="width:20%;">
                                            <%--<input type="submit" name="submitButton" value="Pulisci" onclick="javascript: PulisciPF();return false;">--%>
                                            <%--<input type="submit" name="submitButton" value="Save" onclick="javascript:return ControlloLiquidita();"> --%>                                            
                                            <span style="margin-right: 10px;" class="btBk"><span class="inCn"><input type="submit" name="SubmitButton" value="Salva"  onclick="javascript:return ControlloDati();"/></span></span>                                                   
                                        </td>
                                    </tr>
                                </table>
                                <div style="visibility:hidden">
                                    <input type="submit" name="SubmitButton" value="Precedente" id="btnPrecedente" />
                                </div>                                          
                        </div>
                    </td>
                </tr>                
            </table>            
            <%--<div style="padding: 20px 40px 0 0;text-align:right;">      
                    
                <span style="margin-right: 10px;" class="btBk"><span class="inCn"><input type="submit" name="SubmitButton" value="Precedente" /></span></span>
                <span style="margin-right: 10px;" class="btBk"><span class="inCn"><input type="submit" name="SubmitButton" value="Successivo" /></span></span>                  
            </div>  --%>                      
            <%--<span id="ProvaClasse">
                PROVA CLASSE
            </span>--%> 
        </div>
        
    <%  }%>
        </div>  
    <%   } 
    %>

</asp:Content>



<asp:Content ID="Content8" ContentPlaceHolderID="MenuContent" runat="server">
    <% Html.RenderPartial("~/Views/Shared/MenuStatic.ascx","3;0"); %>
</asp:Content>
