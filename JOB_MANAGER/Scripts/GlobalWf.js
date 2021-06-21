var Classes = [
    { Name: "Primary", Class: "primary", id: 1 },
    { Name: "Secondary", Class: "secondary", id: 2 },
    { Name: "Success", Class: "success", id: 3 },
    { Name: "Danger", Class: "danger", id: 4 },
    { Name: "Warning", Class: "warning", id: 5 },
    { Name: "Info", Class: "info", id: 6 },
    { Name: "Dark", Class: "dark", id: 8 },
    { Name: "Blue", Class: "blue", id: 9 },
    { Name: "Indigo", Class: "indigo", id: 10 },
    { Name: "Purple", Class: "purple", id: 11 },
    { Name: "Pink", Class: "pink", id: 11 },
    { Name: "Red", Class: "red", id: 12 },
    { Name: "Orange", Class: "orange", id: 13 },
    { Name: "Yellow", Class: "yellow", id: 13 },
    { Name: "Green", Class: "green", id: 14 },
    { Name: "Teal", Class: "teal", id: 15 },
    { Name: "Cyan", Class: "cyan", id: 16 },
    { Name: "White", Class: "white", id: 17 },
    { Name: "Gray", Class: "gray", id: 18 },
    { Name: "Gray-Dark", Class: "gray-dark", id: 19 }
];

function showHideLoader(IsShow) {

    if (IsShow == true) {
        if ($('.active').hasClass("sk-loading") == false)
            $('.active').addClass("sk-loading");
    }
    else {
        $('.active').removeClass("sk-loading");
    }    
};

function IsUndefinedOrNullOrEmpty(obj, ignoreCase, isDate1970Control) {

    if (obj == undefined) {
        return true;
    }

    if (obj == null) {
        return true;
    }

    if (typeof obj === 'string') {
        if (obj.toString() == "" || obj.toString() == "null") {
            return true;
        }
    }

    if (isDate1970Control == true) {
        if ($.type(obj) == "date") {
            if (obj.getFullYear() <= 1970) {
                return true;
            }
        }
    }

    if (ignoreCase != undefined && ignoreCase != null && ignoreCase != '') {
        if (obj.toString() == ignoreCase) {
            return true;
        }
    }

    return false;
}

//DateFied
var dateField = function (config) {
    jsGrid.Field.call(this, config);
};

dateField.prototype = new jsGrid.Field({

    sorter: function (date1, date2) {
        return new Date(date1) - new Date(date2);
    },

    itemTemplate: function (value) {
        return IsUndefinedOrNullOrEmpty(value) ? "" : new Date(value).toDateString();
    },

    insertTemplate: function (value) {
        if (!this.inserting)
            return "";

        return this.insertControl = this._createDatePicker();
    },

    editTemplate: function (value) {
        if (!this.editing)
            return this.itemTemplate.apply(this, arguments);

        var $result = this.editControl = this._createDatePicker();
        $result.val(value);
        return $result;
    },

    filterTemplate: function () {
        if (!this.filtering)
            return "";

        var grid = this._grid,
            $result = this.filterControl = this._createDatePicker();    

        return $result;
    },

    filterValue: function () {
        return this.filterControl.val();
    },

    insertValue: function () {
        return this.insertControl.val();
    },

    editValue: function () {
        return this.editControl.val();
    },

    _createDatePicker: function () {
        return $("<input>").attr("type", "date").addClass("form-control")
            .prop("readonly", !!this.readOnly);
    }
});

jsGrid.fields.date = dateField;
//DateFied

//ColorField
var colorField = function (config) {
    jsGrid.Field.call(this, config);
};

colorField.prototype = new jsGrid.Field({

    //sorter: function (date1, date2) {
    //    return new Date(date1) - new Date(date2);
    //},

    itemTemplate: function (value) {
        return IsUndefinedOrNullOrEmpty(value) ? "" : value;
    },

    filterTemplate: function () {
        if (!this.filtering)
            return "";

        var grid = this._grid,
            $result = this.filterControl = this._createColorPicker();

        if (this.autosearch) {
            $result.on("keypress", function (e) {
                if (e.which === 13) {
                    grid.search();
                    e.preventDefault();
                }
            });
        }

        return $result;
    },
    insertTemplate: function (value) {
        if (!this.inserting)
            return "";

        return this.insertControl = this._createColorPicker();
    },

    editTemplate: function (value) {
        if (!this.editing)
            return this.itemTemplate.apply(this, arguments);

        var $result = this.editControl = this._createColorPicker();
        $result.val(value);
        return $result;
    },


    filterValue: function () {
        return this.filterControl.val();
    },

    insertValue: function () {
        return this.insertControl.val();
    },

    editValue: function () {
        return this.editControl.val();
    },

    _createColorPicker: function () {
        return $("<input>").attr("type", "color").addClass("form-control")
            .prop("readonly", !!this.readOnly);
    }
});

jsGrid.fields.color = colorField;
//ColorField

//FileFied
var fileField = function (config) {
    jsGrid.Field.call(this, config);
};

fileField.prototype = new jsGrid.Field({

    itemTemplate: function (value) {
        return value.name;
    },

    insertTemplate: function (value) {
        if (!this.inserting)
            return "";

        return this.insertControl = this._createFilePicker();
    },

    editTemplate: function (value) {
        if (!this.editing)
            return this.itemTemplate.apply(this, arguments);

        var $result = this.editControl = this._createFilePicker();
        $result.val();
        return $result;
    },

    insertValue: function () {
        return this.insertControl[0].files[0];
    },

    editValue: function () {
        return this.editControl[0].files[0];
    },

    _createFilePicker: function () {
        return $("<input>").attr("type", "file").addClass("form-control")
            .prop("readonly", !!this.readOnly);
    }
});
jsGrid.fields.file = fileField;
//FileFied

//selectNew
var MultiselectField = function (config) {
    jsGrid.Field.call(this, config);
};

MultiselectField.prototype = new jsGrid.Field({

    items: [],
    valueField: 0,
    textField: "",
    multiple: false,

    itemTemplate: function (value) {

        if (IsUndefinedOrNullOrEmpty(value) == false && value.length > 0 && this.items.length > 0)
        {
            var Texts = [];
            var valueField = this.valueField;
            var textField = this.textField;

            for (var i = 0; i < value.length; i++) {
                $.each(this.items, function (_, item) {
                    var val = item[valueField];
                    var text = item[textField];

                    if (value[i] == val)
                        Texts.push(text);
                });
            }

            return $.makeArray(Texts).join(", ");
        }

        return "";        
    },
    filterTemplate: function () {
        if (!this.filtering)
            return "";

        var $result = this.filterControl = this._createSelect();

        setTimeout(function () {
            $("select[data-container='body']").selectpicker('refresh');
            FixSelectPicker();
        });
          
        return $result;
    },
    filterValue: function () {
        return this.filterControl.val();
    },

    _createSelect: function (selected) {
        var valueField = this.valueField;
        var textField = this.textField;
        var Witdh = this.width + "px";
        var $result = $("<select>").prop("multiple", this.multiple).addClass("selectpicker")
            //.attr("data-width", Witdh)
            .attr("data-container", "body")
            .attr("data-selected-text-format", "count");

     
        $.each(this.items, function (_, item) {
            var value = item[valueField];
            var text = item[textField];
            var $opt = $("<option>").text(text).attr("value", value);

            if ($.inArray(value, selected) > -1) {
                $opt.attr("selected", "selected");
            }

            $result.append($opt);
        });        
        
        return $result;
    },

    insertTemplate: function () {
        var insertControl = this._insertControl = this._createSelect();

        setTimeout(function () {
            $("select[data-container='body']").selectpicker('refresh');
            FixSelectPicker();
        });

        return insertControl;
    },

    editTemplate: function (value) {
        var editControl = this._editControl = this._createSelect(value);

        setTimeout(function () {
            $("select[data-container='body']").selectpicker('refresh');
            FixSelectPicker();
        });
        
        return editControl;
    },

    insertValue: function () {
        var Val = [];

        $.each(this._insertControl.find("option:selected"), function (_, item) {
            Val.push($(this).val());
        });

        return Val;
    },

    editValue: function () {
        var Val = [];

        $.each(this._editControl.find("option:selected"), function (_, item) {
            Val.push($(this).val());
        });

        return Val;
    },

});

jsGrid.fields.multipleSelect = MultiselectField;


function CreateLoader(element,IsShow)
{   
    if (IsShow == true)
    {
        var htmlGrid = '<div class="sk-spinner sk-spinner-double-bounce">'
            + '<div class="sk-double-bounce1"></div>'
            + '<div class="sk-double-bounce2"></div>'
            + '</div>';

        element.append(htmlGrid).addClass("spinner").addClass("sk-loading");
        //$('.active').addClass("sk-loading");
    }
    else
    {
        element.removeClass("spinner").removeClass("sk-loading");
        element.find(".sk-spinner").remove();

    }
};

function GetParamVal(paramName, Type)
{
    var parameters = localStorage.getItem('Parameters');
    var paramVal = null;
    if (IsUndefinedOrNullOrEmpty(parameters) == false) {
        var objParam = JSON.parse(parameters);

        var Find = -1;
        $.each(objParam, function (i, item)
        {
            Find = 1;
            if (item.PARAM_NAME == paramName)
            {
                if (Type == 1) {
                    paramVal = item.PARAM_INT;
                    Find = 2;
                }
                else
                if (Type == 2) {
                    paramVal = item.PARAM_IMG;
                    Find = 2;
                }
                else
                if (Type == 3) {
                    paramVal = item.PARAM_STR;
                    Find = 2;
                }

                
            }
        });        

        if (Find > 0)
            return paramVal;
    }
    else
    {
        return paramVal;
    }    
}


function CreateHtmlJsGrid(Name, callback) {

    var defPageSize = 25;
    var ParamVal = GetParamVal("GRID_DEFAULT_PAGER_RANGE",1);
    if (IsUndefinedOrNullOrEmpty(ParamVal) == false)
        defPageSize = ParamVal;

    var option = '';
    option += defPageSize == 5 ? '<option selected>5</option>' : '<option>5</option>'
    option += defPageSize == 10 ? '<option selected>10</option>' : '<option>10</option>'
    option += defPageSize == 25 ? '<option selected>25</option>' : '<option>25</option>'
    option += defPageSize == 50 ? '<option selected>50</option>' : '<option>50</option>';

    var Customs = Name.search(" ") <= 0 ? "" : Name.substring(Name.search(" "), Name.length).trim();
    Name = Name.search(" ") <= 0 ? Name : Name.substring(0, Name.search(" ")).trim();

    var CreateOwnFunction = false;
    if (Customs.indexOf("CreateOwnFunction") >= 0) CreateOwnFunction = true;

    var ToolBarFalse = false;
    if (Customs.indexOf("ToolBarFalse") >= 0) {
        ToolBarFalse = true;
        Customs = "";
    }

    var ChangeFunctionName = "";
    if (CreateOwnFunction) {
        ChangeFunctionName = Name + 'ChangeGrid(this)';
        Customs = "";
    }
    else
    {
        ChangeFunctionName = 'ChangeGrid(this)';
    }
   
    if (ToolBarFalse)
        var htmlGrid = '<div id="Table' + Name + '" class="text-primary"></div>';
    else
        var htmlGrid = '<div class="sk-spinner sk-spinner-double-bounce">'
            + '<div class="sk-double-bounce1"></div>'
            + '<div class="sk-double-bounce2"></div>'
            + '</div>'
            + '<div class="row">'
            + '<div class="col-sm-4">'
            + '<label for="pageSize">Page Size:</label>'
            + '<select id="pageSize" class="form-control form-control-sm selectpicker" data-width="fit" onchange="' + ChangeFunctionName+'">'
            + option
            + '</select>'
            + Customs
            + '</div>'
            + '<div class="col-sm-8">'
            + '<div class="btn-toolbox float-sm-right">'
            + '<button onClick="' + (CreateOwnFunction ? Name + 'RefreshGrid(this)' : 'RefreshGrid(this)') +'" class="btn btn-info btn-sm RefreshItem" style="margin-right:5px"><i class="fas fas fa-sync"></i> Refresh</button>'
            + '<button onClick="' + (CreateOwnFunction ? Name + 'AddGrid(this)' : 'AddGrid(this)') +'" class="btn btn-success btn-sm addItem" style="margin-right:5px"><i class="fas fa-plus-circle"></i> Add New Item</button>'
            + '<button onClick="' + (CreateOwnFunction ? Name + 'SearchGrid(this)' : 'SearchGrid(this)') +'" class="btn btn-primary btn-sm searchItem" style="margin-right:5px"><i class="fas fa-plus-circle"></i> Search Item</button>'
            + '</div>'
            + '</div>'
            + '</div>'         
            + '<div id="Table'+Name+'" class="text-dark"></div>';

    callback(htmlGrid);
}


function CreateJsGrid(Options,callback)
{
    //Options.Element
    //Options.Data
    //Options.Fields
    //Options.DbOperation.Tablename
    //Options.DbOperation.GetUrl
    //Options.DbOperation.AddUrl
    //Options.DbOperation.DeleteUrl
    

    var parameters = localStorage.getItem('Parameters');
    var defPageSize = 25;
    var ParamVal = GetParamVal("GRID_DEFAULT_PAGER_RANGE", 1);
    if (IsUndefinedOrNullOrEmpty(ParamVal) == false)
        defPageSize = ParamVal;


    var jsgridDataFlg = false;
    var t1 = Options.Name.jsGrid({
        height: IsUndefinedOrNullOrEmpty(Options.Width) ? "400px" : Options.Width,
        width:  "100%",

        editing: true,
        sorting: true,
        paging: true,
        autoload: true,
        confirmDeleting: true,

        deleteConfirm: "Are you sure?",
        noDataContent: "There is not description. Add a new one",

        pageIndex: 1,
        pageSize: defPageSize,
        pageButtonCount: 15,
        pagerFormat: "Pages: {first} {prev} {pages} {next} {last}    {pageIndex} of {pageCount}",
        pagePrevText: "Prev",
        pageNextText: "Next",
        pageFirstText: "First",
        pageLastText: "Last",
        pageNavigatorNextText: "...",
        pageNavigatorPrevText: "...",


        //rowClass: function (item, itemIndex)
        //{ 
        //    if (Options.RowClass && Options.Type == 1)
        //    {
        //        return "phase-" + item.PHASE_ID;
        //    }
        //},        
      
        controller: {
            data: Options.Data,
            loadData: function (filter) {

                $('.selectpicker').selectpicker('refresh');
                $('.bs-caret span').removeClass("caret");

                var all_empty = true;
                for (var field in filter) {
                    if (filter[field]) {
                        all_empty = false;
                    }
                }

                if (IsUndefinedOrNullOrEmpty(Options.Url) == true && all_empty)
                {               
                    jsgridDataFlg = true;
                    return;
                }

                var d = $.Deferred();
                

                if (IsUndefinedOrNullOrEmpty(Options.Data) == false && Options.Data.length > 0 && all_empty == false) {
                    FilterGrid(Options.Data, filter, function (filterData) {
                        d.resolve(filterData);
                    });
                }              
                else
                {
                    var requestdata = IsUndefinedOrNullOrEmpty(Options.LoadParam) ? null : $.param(Options.LoadParam);

                    $.ajax({
                        url: Options.Url,
                        data: requestdata,
                        dataType: "json"
                    }).done(function (response) {
                        setTimeout(function () {
                            data = response.Getlist;
                            d.resolve(response.Getlist);
                        }, 2000);
                    });
                }

                return d.promise();
            },
        },

        onItemInserting: function (args) {

            OnAddNewItem(args);
        },

        onItemUpdating: function (args) {

            OnAddNewItem(args);
        },

        onItemDeleting: function (args) {
            OnDeleteITem(args);
        },

        onRefreshed: function () {
            var $gridData = Options.Name.find(".jsgrid-grid-body tbody");

            if (Options.RowClass && Options.Type == 1)
            {
                $gridData.sortable({
                    update: function (e, ui) {
                        // array of indexes
                        //var clientIndexRegExp = /\s*client-(\d+)\s*/;
                        //var indexes = $.map($gridData.sortable("toArray", { attribute: "class" }), function (classes) {
                        //    return clientIndexRegExp.exec(classes)[1];
                        //});
                        //alert("Reordered indexes: " + indexes.join(", "));

                        // arrays of items
                        var items = $.map($gridData.find("tr"), function (row,index)
                        {
                            var item = $(row).data("JSGridItem");
                            item.PHASE_ORDER = index;
                            return item;
                        });
                        console && console.log("Reordered items", items);
                    }
                });
            }           
        },

        onDataLoaded: function (args) {
            Options.Data = args.data;

            CreateTooltip();
            FixSelectPicker();
            $('.selectpicker').selectpicker('refresh');            

            var retOption = {};
            retOption.Grid = t1;
            retOption.Data = Options.Data;

            callback(retOption);
        },

        fields: Options.Fields

    });


    setTimeout(function ()    
    {
        if (jsgridDataFlg)
        {
            var retOption = {};
            retOption.Grid = t1;
            retOption.Data = Options.Data;

            callback(retOption);
        }
    });

}


function FilterGrid(data, filter,callback) 
{
    var matched = [];
    $.grep(data, function (item) {        
        var fields_to_search = [];
        // Add fields to search in order to allow multiple fields searching
        for (var field in filter) {

            // Ignore the search in empty fields
            if (!filter[field]) {
                continue;
            }
            else
            if (Array.isArray(filter[field]) && emptyArray.length <= 0)
                continue;

            fields_to_search.push(field);
        }

        // Check if all the fields match
        for (var i = 0; i < fields_to_search.length; i++)
        {
            var field = fields_to_search[i];
            if (typeof filter[field] == "boolean") {
                if (filter[field] == item[field]) {
                    matched.push(item);
                    break;
                }
            }
            else if (typeof filter[field] == "number") {
                if (filter[field] == item[field]) {
                    matched.push(item);
                    break;
                }
            }
            else if (Array.isArray(filter[field]))
            {
                var find = false; 

                if (IsUndefinedOrNullOrEmpty(item[field]) == false)
                {
                    for (var i = 0; i < item[field].length; i++)
                    {
                        //var result = filter[field].filter(f => f = item[field][i]);
                        for (var y = 0; y < filter[field].length; y++)
                        {
                            if (filter[field][y] == item[field][i])
                            {
                                matched.push(item);
                                find = true;
                                break;
                            }
                        }
                    }
                }

                if (find)
                    break;
            }
            else
            {
                if (IsUndefinedOrNullOrEmpty(item[field]) == true)
                    item[field] = "";

                if ((item[field].toLowerCase().indexOf(filter[field].toLowerCase()) >= 0)) {
                    matched.push(item);
                    break;
                }
            }
        }
        
    });

    callback(matched);
}

function EditProfile(elementForm,elementCard) {
    $(elementForm).toggleClass("displaynone");

    $(elementCard).slideToggle();//
}

function ScaleProfile(element) {

    if ($(element).hasClass("maximized-card")) {

        $(element).removeClass("maximized-card");
    }
    else {
        $(element).addClass("maximized-card");
    }
}

function pwstrength(element)
{
    "use strict";
    var options = {};
    options.common = {
        onKeyUp: function (evt, data) {
            dataScoreUpd(data.score);//dataScrore = data.score;
        }
    }
    options.ui = {
        container: "#pwd-container",
        showStatus: false,
        showVerdictsInsideProgressBar: true,
        viewports: {
            progress: ".pwstrength_viewport_progress"
        }
    };
    options.rules = {
        activated: {
            wordNotEmail: true,
            wordRepetitions: true,
            wordUpperLowerCombo: true,
            wordLetterNumberCombo: true
        },
    };

    $(element).pwstrength(options);    
}

function CreateTooltip()
{
    $('[data-toggle="tooltip"]').tooltip({
        show: {
            effect: "slideDown",
            delay: 250
        },
        content: function () {
            return $(this).attr('title');
        }
    });
}

function ShowLink(link) {
    if (link == "") {
        toastr.warning("Link is empty", "Warning");
        return;
    }

    window.open(link, "", "width=300, height=300");
}

function Temizle(element) {
    element.selectpicker('val', -1);
}

function FormReset(element) {
    element.trigger("reset");
    $('.card-footer input:hidden').val("0");
    $('.selectpicker').each(function (index)
    {
        if ($(this).attr("id").indexOf("STATE") >= 0 || $(this).attr("id").indexOf("CITY") >= 0)
            $(this).find('option').remove();

        $(this).selectpicker('refresh');
    });
}

function lightOrDark(color) {
    // Variables for red, green, blue values
    var r, g, b, hsp;
    // Check the format of the color, HEX or RGB?
    if (color.match(/^rgb/)) {
        // If HEX --> store the red, green, blue values in separate variables
        color = color.match(/^rgba?\((\d+),\s*(\d+),\s*(\d+)(?:,\s*(\d+(?:\.\d+)?))?\)$/);
        r = color[1];
        g = color[2];
        b = color[3];
    }
    else {
        // If RGB --> Convert it to HEX: http://gist.github.com/983661
        color = +("0x" + color.slice(1).replace(
            color.length < 5 && /./g, '$&$&'));
        r = color >> 16;
        g = color >> 8 & 255;
        b = color & 255;
    }
    // HSP (Highly Sensitive Poo) equation from http://alienryderflex.com/hsp.html
    hsp = Math.sqrt(
        0.299 * (r * r) +
        0.587 * (g * g) +
        0.114 * (b * b)
    );
    // Using the HSP value, determine whether the color is light or dark
    if (hsp > 127.5) {
        return 'black';
    }
    else {
        return 'white';
    }
}

function FixSelectPicker() {
    $(".dropdown-menu .inner").each(function (index) {
        if ($(this).hasClass('show') == false)
            $(this).addClass('show');
    });

    $('.bs-caret span').removeClass("caret");
}
