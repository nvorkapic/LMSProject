var tableWidth = 720;
var tableHeight = 1000;
var tableOffsetX = 33;
var tableOffsetY = 0;
var cellDimensionsX = 100;
var cellDimensionsY = 50;
var daysOfTheWeek = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
var hourOffset = -4;

var drawScheduleDetails = function (smodels) {
    context.beginPath();

    for (var i = 0; i < smodels.length; i++) {
        context.lineWidth = 1;
        context.strokeStyle = 'black';
        context.fillStyle = "#D0EAF2";
        context.font = "10px Arial";

        var detailx = smodels[i].DayOfTheWeek * cellDimensionsX + tableOffsetX;
        var detaily = (smodels[i].Hours + hourOffset) * cellDimensionsY + tableOffsetY + smodels[i].Minutes * (cellDimensionsY / 60);
        var detailheight = smodels[i].DurationInMinutes * (cellDimensionsY / 60);
        context.fillRect(
				detailx,
				detaily,
				cellDimensionsX,
				detailheight
			);
        context.fillStyle = 'black';
        context.rect(detailx, detaily, cellDimensionsX, detailheight);
        context.fillText(
			smodels[i].TimeDisplayStart,
			detailx + cellDimensionsX - 34,
			detaily + 12);
        context.fillText(
			smodels[i].Label,
			detailx + 1,
			detaily + 12);
        context.fillText(
			"Room " + smodels[i].Room,
			detailx + 1,
			detaily + 24);
        if (smodels[i].Task != "") {
            context.fillStyle = 'red';
            context.fillText(smodels[i].Task,
			detailx + 1,
			detaily + detailheight - 12);
            context.fillStyle = 'black';
        }
        context.fillText(smodels[i].TimeDisplayEnd,
			detailx + cellDimensionsX - 34,
			detaily + detailheight - 3);
    }

    context.stroke();
}

var drawTimetable = function () {
    // Border
    context.beginPath();
    context.rect(tableOffsetX, tableOffsetY, tableWidth, tableHeight);
    context.lineWidth = 1;
    context.strokeStyle = 'black';
    context.fillStyle = 'black';

    // Header
    context.font = "18px Arial";
    for (var i = 0; i < 7; i++) {
        var cellOffsetX = i * cellDimensionsX;
        context.fillText(daysOfTheWeek[i], cellOffsetX + tableOffsetX + 20, tableOffsetY + 30);
    }
    // Draw the schedule posts
    drawScheduleDetails(models);

    // Body
    context.lineWidth = 1;
    context.strokeStyle = 'gray';
    context.fillStyle = 'gray';
    context.font = "12px Arial";

    var tableBodyOffsetY = tableOffsetY + cellDimensionsY;
    for (var y = 0; y < 18; y++) {
        var cellY = tableBodyOffsetY + cellDimensionsY * y;
        context.fillStyle = 'black';
        context.fillText((y + 5) + ".00", 0, cellY + 5);
        for (var x = 0; x < 7; x++) {
            var cellX = tableOffsetX + x * cellDimensionsX;
            context.rect(cellX, cellY,
				cellDimensionsX,
				cellDimensionsY
				);
        }
    }

    context.stroke();

    // Draw the schedule posts
    drawScheduleDetails(models);

}


var canvas = document.getElementById('canvas');
var context = canvas.getContext('2d');

var calendar = angular.module("calendar", []);
var models = [];
calendar.controller("CalendarController", function ($scope, $http, $q) {
    $scope.showCreate = false;
    $scope.getScheduleDetails = function () {
        var deferred = $q.defer();
        $http(
		{
		    method: 'GET',
            url: $scope.fetchurl
		    //url: '/Calendar/getSchedule/@Model'
		    // url: '/Calendar/getScheduleForStudent/?userId=@Model'
		}).then(function successCallback(response) {
		    deferred.resolve(response);

		    models = response.data;
		    console.log("Got the data!");
		    console.log(models);
		    drawTimetable();

		    return response.data;
		    // drawTimetable(models);
		},
		function failureCallback(response) {
		    deferred.reject({ message: "Failed to get data" });
		    console.log("Failed to get data");
		});
        return deferred.promise;
    }

    $scope.getScheduleDetails();
});
