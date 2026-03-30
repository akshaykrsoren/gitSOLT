function GetPatientsNetworkData(divName, fromdate, todate) {
    var url = INITURL + '/Dashboard/GetPatientsData';

    $.ajax({
        url: url,
        data: { FromDate: fromdate, ToData: todate },
        cache: false,
        type: "POST",
        success: function (data) {
            var PatientList = JSON.parse(data);

            var bounds = new google.maps.LatLngBounds();
            var infowindow;
            if (PatientList.length == 0) {
                var latlng = new google.maps.LatLng("39.232253", "-99.580078");
            }
            else {
                var latlng = new google.maps.LatLng(PatientList[0].Latitude, PatientList[0].Longitude);
            }
            var myOptions =
            {
                zoom: 3,
                center: latlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var map = new google.maps.Map(document.getElementById(divName), myOptions);
            if (PatientList.length > 0) {
            for (var i = 0; i < PatientList.length; i++) {
                var innerMarkerText = '';
                innerMarkerText += '<table><tr><td style="text-align:center;padding:5px;color: #088BCA">' + PatientList[i].Name + '</td></tr>';
                innerMarkerText += '<tr><td style="text-align:center;padding:5px;color: #088BCA">' + PatientList[i].Email + '</td></tr>';
                innerMarkerText += '<tr><td style="text-align:center;padding:5px;color: #088BCA">' + PatientList[i].Phone + '</td></tr></table>';

                var latlng = new google.maps.LatLng(PatientList[i].Latitude, PatientList[i].Longitude);
                var myMarker = new google.maps.Marker(
                {
                    position: latlng,
                    map: map,
                    icon: "../image/marker.png"
                });

                myMarker['infowindow'] = new google.maps.InfoWindow({
                    content: innerMarkerText
                });

                google.maps.event.addListener(myMarker, 'mouseover', function () {
                    this['infowindow'].open(map, this);
                });

                google.maps.event.addListener(myMarker, 'mouseout', function () {
                    this['infowindow'].close(map, this);
                });

                bounds.extend(myMarker.position);
            }
                // Don't zoom in too far on only one marker
                if (bounds.getNorthEast().equals(bounds.getSouthWest())) {
                    var extendPoint1 = new google.maps.LatLng(bounds.getNorthEast().lat() + (0.001), bounds.getNorthEast().lng() + (0.001));
                    var extendPoint2 = new google.maps.LatLng(bounds.getNorthEast().lat() - (0.001), bounds.getNorthEast().lng() - (0.001));
                    bounds.extend(extendPoint1);
                    bounds.extend(extendPoint2);
                }
            map.fitBounds(bounds);
            }
        },
        error: function (reponse) {
            bootbox.alert("Something went wrong.");
        }
    });
}

function GetNurseNetworkData(divName, fromdate, todate) {
    var url = INITURL + '/Dashboard/GetNurseData';



    $.ajax({
        url: url,
        data: { FromDate: fromdate, ToData: todate },
        cache: false,
        type: "POST",
        success: function (data) {
            var NurseList = JSON.parse(data);

            var infowindow;
            var bounds = new google.maps.LatLngBounds();

            if (NurseList.length == 0) {
                var latlng = new google.maps.LatLng("39.232253", "-99.580078");
            }
            else { 
                var latlng = new google.maps.LatLng(NurseList[0].Latitude, NurseList[0].Longitude);
            }
            var myOptions =
            {
                zoom: 5,
                center: latlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var map = new google.maps.Map(document.getElementById(divName), myOptions);

            if (NurseList.length != 0) {
                for (var i = 0; i < NurseList.length; i++) {
                    var isAvailable = "";
                    if (NurseList[i].IsAvailable == true) {
                        isAvailable = "Available";
                    }
                    else {
                        isAvailable = "Not Available";
                    }
                    var innerMarkerText = '';
                    innerMarkerText += '<table><tr><td style="text-align:center;color: #088BCA;">' + NurseList[i].Name + '</td>';
                    innerMarkerText += '<td rowspan="4" style="padding: 10px;">'
                    innerMarkerText += '<img src="' + NurseList[i].ProfileImage + '" width="100" height="100" style="border:1px solid #088BCA; margin-right:10px" /></td></tr>';
                    innerMarkerText += '<tr><td style ="text-align:center;">Hours Rate<br />(Charged to Patient)</td></tr>';
                    innerMarkerText += '<tr><td style="text-align:center;color: #088BCA;"><b>$ ' + NurseList[i].HourlyRate + '</b></td></tr>';
                    innerMarkerText += '<tr><td style="text-align:center;color: #088BCA;">Available</td></tr></table>';

                    var latlng = new google.maps.LatLng(NurseList[i].Latitude, NurseList[i].Longitude);
                    var myMarker = new google.maps.Marker(
                    {
                        position: latlng,
                        map: map,
                        icon: "../image/marker.png"
                    });

                    myMarker['infowindow'] = new google.maps.InfoWindow({
                        content: innerMarkerText
                    });

                    google.maps.event.addListener(myMarker, 'mouseover', function () {
                        this['infowindow'].open(map, this);
                    });

                    google.maps.event.addListener(myMarker, 'mouseout', function () {
                        this['infowindow'].close(map, this);
                    });

                    bounds.extend(myMarker.position);
                    //loadScript("/Scripts/js/MapLibrary.js", "MapLibraryjs", "js", function () {
                    //    SetMapByLatitudeLongitude("DivNurseNetwork", NurseList[i].Latitude, NurseList[i].Longitude, null, innerMarkerText, null)
                    //});
                }
                // Don't zoom in too far on only one marker
                if (bounds.getNorthEast().equals(bounds.getSouthWest())) {
                    var extendPoint1 = new google.maps.LatLng(bounds.getNorthEast().lat() + (0.001), bounds.getNorthEast().lng() + (0.001));
                    var extendPoint2 = new google.maps.LatLng(bounds.getNorthEast().lat() - (0.001), bounds.getNorthEast().lng() - (0.001));
                    bounds.extend(extendPoint1);
                    bounds.extend(extendPoint2);
            }
            map.fitBounds(bounds);
            }
        },
        error: function (reponse) {
            bootbox.alert("Something went wrong.");
        }
    });
}

