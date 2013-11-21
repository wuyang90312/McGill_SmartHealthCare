<%@ Page Title="graph" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="graph.aspx.cs" Inherits="graph" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">

    <script type="text/javascript">
        var graph;
        var xPadding = 30;
        var yPadding = 30;
        function getMaxY(val) {
            var max = 0;

            for (var i = 0; i < val.length; i++) {
                if (val[i] > max) {
                    max = val[i];
                }
            }
            max += 10 - max % 10;
            return max;
        }
        function getXPixel(val, length) {
            return ((graph.width() - xPadding) /length) * val + (xPadding * 2);
        }
        function getYPixel(val, MAX) {
            return graph.height() - val * ((graph.height()-yPadding)/(MAX+3)) - yPadding;
        }

/********************************************************************************************************/
        var xTime = [];
        var positionX = [];
        var positionY = [];
        var offsetX = 80;
        var offsetY = 10;
        $('div').mousemove(function (event) {
            var clientCoords = "( " + event.clientX + ", " + event.clientY + " )";
            
            var length = xTime.length;
            if ( length > 0)
            {
                var graph = $('#graph');
                var diffX = Math.round((event.clientX - graph.position().left - 2 * xPadding) * length / (graph.width() - xPadding));
                if (diffX<length)
                var diffY = Math.abs(event.clientY - positionY[diffX] - 190);

                var c = graph[0].getContext('2d');
                if (diffY <= 5) {
                    $('#content').css({ left: (event.clientX-100), top: (event.clientY-50) });
                    $("span[id$='Label3']").text(xTime[diffX]);
                } else {
                    $("span[id$='Label3']").text("");
                }
            }
    
        });
/***********************************************************************************************************/

        $(document).ready(function () {
            
            //$("select[id$=selection_vital]").change(function () {
            $("input[id$=applyButton]").click(function () {
                event.preventDefault(); // stops form sumission
                var informationArray = [];

                /*Reinitialize the Xtime*/
                xTime = [];
/***********************************************************************************/
                var listString = $("input[id$='HiddenField1']").val();
                var dateInfo = listString.split(";");
                var output = " ";
                var index = $("select[id$=selection_vital]").val();
                var timestamp = dateInfo[0].split(",");
                var stamp;

                //choose the time scale
                switch ($("select[id$=time_scale]").val()) {
                    case "0":
                        stamp = 1;
                        break;
                    case "1":
                        stamp = timestamp[1];
                        break;
                    case "2":
                        stamp = timestamp[2];
                        break;
                    case "3":
                        stamp = timestamp[3];
                        break;
                    case "4":
                        stamp = timestamp[4];
                        break;
                    default:
                        stamp = timestamp[0];
                        break;
                }
                
                for (var i = stamp; i >0 ; i--)
                {
                    var part = dateInfo[i].split(",");
                    if ( !(index=="5" || part[index]==="---")) {
                        informationArray.push(parseInt(part[index]));
                        xTime.push(part[5]);

                    } else if (index === "5") {
                    } else {
                       // alert("There is no information can be retrieved!");
                    }
                    
                }

                for (var i = 0; i < informationArray.length; i++) {
                        output += i+":" + informationArray[i]+" "+xTime[i];
                    
                }
               
                /****************************************************************************************/
                $("span[id$='Label1']").html("");
                //generate the coordinates
                graph = $('#graph');
                var c = graph[0].getContext('2d');
                //clear the canvas for redrawing
                c.clearRect(0, 0, graph.width(), graph.height());
                // if there is no data, don't generate graph
                if (informationArray.length == 0) {
                    $("span[id$='Label1']").html("There is not usable value in this time scale");
                }
                   
                else {
                    
                    c.lineWidth = 2;
                    c.strokeStyle = '#333';
                    c.font = 'italic 8pt sans-serif';
                    c.textAlign = "center";
                    c.beginPath();
                    c.moveTo(xPadding, 0);
                    c.lineTo(xPadding, graph.height() - yPadding);
                    c.lineTo(graph.width(), graph.height() - yPadding);
                    c.stroke();

                    //define Y-axis
                    c.textAlign = "right";
                    c.textBaseline = "middle";
                    var Max = getMaxY(informationArray);
                    for (var i = 0; i <= Max; i += 10) {
                        c.fillText(i, xPadding - 10, getYPixel(i, Max));
                    }

                    //link the curve
                    c.strokeStyle = '#f00';
                    c.beginPath();
                    c.moveTo(getXPixel(0), getYPixel(informationArray[0]));

                    for (var i = 0; i < informationArray.length; i++) {
                        c.lineTo(getXPixel(i,xTime.length), getYPixel(informationArray[i], Max));
                    }
                    c.stroke();

                    // plot the dots
                    c.fillStyle = '#333';
                    var string;
                    for (var i = 0; i < informationArray.length; i++) {
                        c.beginPath();
                        c.fillText(informationArray[i], getXPixel(i, xTime.length), getYPixel(informationArray[i], Max) - 10);
                        positionX[i] = getXPixel(i, xTime.length);
                        positionY[i] = getYPixel(informationArray[i], Max);
                        c.arc(getXPixel(i, informationArray.length), getYPixel(informationArray[i], Max), 4, 0, Math.PI * 2, true);
                        c.fill();
                    }
                    
                }
                
            });
        });
    </script>

    <asp:dropdownlist runat="server" name="selection_vital" id ="selection_vital">
        <asp:ListItem value="5">Please select one vital sign</asp:ListItem>
        <asp:ListItem value="0">Temperature</asp:ListItem>
        <asp:ListItem value="1">SYS NIBP</asp:ListItem>
        <asp:ListItem value="2">DIA NIBP</asp:ListItem>
        <asp:ListItem value="3">Heart Rate</asp:ListItem>
        <asp:ListItem value="4">Sat Respiration</asp:ListItem>
    </asp:dropdownlist>
     <asp:dropdownlist runat="server" name="time_scale" id ="time_scale">
        <asp:ListItem value="5">Please select the time scale</asp:ListItem>
        <asp:ListItem value="0">12 hrs</asp:ListItem>
        <asp:ListItem value="1">24 hrs</asp:ListItem>
        <asp:ListItem value="2">36 hrs</asp:ListItem>
        <asp:ListItem value="3">48 hrs</asp:ListItem>
        <asp:ListItem value="4">One Month</asp:ListItem>
    </asp:dropdownlist>
    <asp:Button runat="server" ID="applyButton" name ="apply" Text="APPLY"/>
    <asp:Label ID="Label1" runat="server" > </asp:Label>
    <div>
    <asp:HiddenField ID="HiddenField1" runat="server" />
    </div>
    <br>
    <canvas id="graph" style="top: 0; left:0;position: relative;z-index: 1" width="1200" height="400">   
        </canvas>
    <div id="content" style="position:absolute; z-index: 2; margin: 50px">
        <asp:Label ID="Label3" runat="server"> </asp:Label>
    </div>
</asp:Content>

