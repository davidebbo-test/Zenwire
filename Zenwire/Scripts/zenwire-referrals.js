function formatCurrency(value) {
    return "$" + value.toFixed(2);
}

var PayCodeLine = function () {
    var self = this;
    self.payCode = ko.observable();
    self.quantity = ko.observable(1);
    self.subtotal = ko.computed(function () {
        return self.payCode() ? self.payCode().price * parseInt("0" + self.quantity(), 10) : 0;
    });
};

var PayCode = function () {
    // Stores an array of lines, and from these, can work out the grandTotal
    var self = this;
    self.lines = ko.observableArray([new PayCodeLine()]); // Put one line in by default
    self.grandTotal = ko.computed(function () {
        var total = 0;
        $.each(self.lines(), function() { total += this.subtotal(); });
        return total;
    });

    // Operations
    self.addLine = function () { self.lines.push(new PayCodeLine()); };
    self.removeLine = function (line) { self.lines.remove(line); };
    self.save = function () {
        var dataToSave = $.map(self.lines(), function (line) {
            return line.payCode() ? {
                productName: line.payCode().name,
                quantity: line.quantity()
            } : undefined;
        });
        alert("Could now send this to server: " + JSON.stringify(dataToSave));
    };
};

ko.applyBindings(new PayCode());