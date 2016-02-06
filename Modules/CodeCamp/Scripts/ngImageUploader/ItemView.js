var ngImgUploader = ngImgUploader || {};

ngImgUploader.itemListViewModel = function (moduleId, resx) {
    var service = {
        path: "ngImgUploader",
        framework: $.ServicesFramework(moduleId)
    }
    service.baseUrl = service.framework.getServiceRoot(service.path) + "Item/";

    var isLoading = ko.observable(false);
    var itemList = ko.observableArray([]);
    var editMode = ko.computed(function () {
        return itemList().length > 0 && itemList()[0].editUrl().length > 0;
    });

    var init = function () {
        getItemList();
    };

    var getItemList = function () {
        isLoading(true);
        var jqXHR = $.ajax({
            url: service.baseUrl,
            beforeSend: service.framework.setModuleHeaders,
            dataType: "json"
        }).done(function (data) {
            if (data) {
                load(data);
            }
            else {
                // No data to load 
                itemList.removeAll();
            }
        }).always(function (data) {
            isLoading(false);
        });
    };

    var deleteItem = function (item) {
        isLoading(true);
        var restUrl = service.baseUrl + item.id();
        var jqXHR = $.ajax({
            method: "DELETE",
            url: restUrl,
            beforeSend: service.framework.setModuleHeaders
        }).done(function () {
            console.log("Deleted: " + item.id());
            itemList.remove(item);
        }).fail(function () {
            console.log("Error");
        }).always(function (data) {
            console.log("finished");
            isLoading(false);
        });
    };

    var load = function (data) {
        itemList.removeAll();
        var underlyingArray = itemList();
        for (var i = 0; i < data.length; i++) {
            var result = data[i];
            var item = new ngImgUploader.ItemDTO();
            item.load(result);
            underlyingArray.push(item);
        }
        itemList.valueHasMutated();
    };

    var alertSample = function () {
        alert("Hello World!");
    };

    return {
        init: init,
        load: load,
        itemList: itemList,
        getItemList: getItemList,
        deleteItem: deleteItem,
        editMode: editMode,
        isLoading: isLoading,
        alert: alertSample
    }
};

ngImgUploader.ItemDTO = function () {
    var id = ko.observable("");
    var name = ko.observable("");
    var description = ko.observable("");
    var assignedUser = ko.observable(-1);
    var editUrl = ko.observable("");

    var load = function (data) {
        id(data.id)
        name(data.name);
        assignedUser(data.assignedUserId);
        description(data.description);
        editUrl(data.editUrl);
    };

    return {
        id: id,
        name: name,
        description: description,
        editUrl: editUrl,
        load: load
    }
}
