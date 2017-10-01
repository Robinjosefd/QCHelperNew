<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" EnableViewState="true" EnableSessionState="true" Inherits="QCHelperNew.Default1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager ID="scriptmanager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Timer ID="tmr" Enabled="true" runat="server" Interval="10000" OnTick="tmr_Tick">
                </asp:Timer>
                <br />
                <br />



                <%-- <asp:Image ID="imgLoadingRetran" runat="server" ImageUrl="~/images/loading_Transparent.gif" />--%>

                <div>
                    <%--DivTimer Start --%>
                    <div>
                        <div id="divtimedisplay">
                            <table border="1">
                                <tr>
                                    <td>
                                        <b>
                                            <asp:Label ID="Label1" runat="server" Text="Appharbor Time : "></asp:Label>
                                        </b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAppharborTime" runat="server" Text="Appharbor"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <b>
                                            <asp:Label ID="Label2" runat="server" Text="PST Time : "></asp:Label>
                                        </b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPsttime" runat="server" Text="PST"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <b>
                                            <asp:Label ID="Label3" runat="server" Text="Indian Time : "></asp:Label>
                                        </b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblIST" runat="server" Text="Indian"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <p>&nbsp;</p>
                            <table>
                                <tr class="font">
                                    <td>
                                        <b>Date:</b>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDate" runat="server">
                                        </asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnShowQC" runat="server" OnClick="btnShowQC_Click" Text="Get QC"
                                            Width="80px" theme="Glass"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <%--DivTimer End --%>

                    <br />

                    <%--Div QCCounts  Start --%>
                    <div>
                        <asp:Label ID="lblError" runat="server" Text="Morning Rundown" Font-Bold="true" ForeColor="Red">  </asp:Label>


                        <asp:GridView ID="dxGrdCount" runat="server" theme="Glass">
                        </asp:GridView>

                        <br />
                        <asp:GridView ID="dxGrdCount_2" runat="server" theme="Glass">
                        </asp:GridView>
                        <br />
                        <asp:GridView ID="dxGrdCount_3" runat="server" theme="Glass">
                        </asp:GridView>
                        <br />
                        <asp:GridView ID="dxGrdCount_4" runat="server" theme="Glass">
                        </asp:GridView>
                    </div>
                    <br />
                    <%--Div QCCounts  End --%>


                    <%--Div QCCSV   Start --%>
                    <div>
                        <asp:Label ID="lblCSVCreated" runat="server" Text="CSV Files Created" Font-Bold="true" ForeColor="Red">  </asp:Label>

                        <asp:GridView ID="dxGrdQCCSV1" runat="server" theme="Glass">
                        </asp:GridView>

                        <asp:GridView ID="dxGrdQCCSV2" runat="server" theme="Glass">
                        </asp:GridView>
                    </div>
                    <%--Div QCCSV  End --%>


                     <br />
                    <%--Div Visual Indicator   Start --%>
                    <div>
                        <asp:Label ID="lblVisualIndicator" runat="server" Text="Visual Indicator Functioning" Font-Bold="true" ForeColor="Red">  </asp:Label>

                        <asp:GridView ID="dxGrdVisualIndicator" runat="server" theme="Glass">
                        </asp:GridView>
                    </div>
                    <%--Div QCLog  End --%>


                    <br />

                    <%--Div QCLog   Start --%>
                    <div>
                        <asp:Label ID="lblQClog" runat="server" Text="Log Created" Font-Bold="true" ForeColor="Red">  </asp:Label>

                        <asp:GridView ID="dxGrdRobotLog" runat="server" theme="Glass">
                        </asp:GridView>
                    </div>
                    <%--Div QCLog  End --%>

                    
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
    <p>
        &nbsp;
    </p>
</body>
</html>
