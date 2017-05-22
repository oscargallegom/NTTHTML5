<%@ Page Title="Modifications" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Modifications.aspx.vb" Inherits="NTTHTML5.Modifications"%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl ("~/Scripts/modifications.js") %>"></script>
    <asp:Label ID="lblFile" Text="File: " runat="server"></asp:Label>
    <asp:DropDownList ID="ddlFile" AutoPostBack="true" runat="server" >
        <asp:ListItem Text="Control File" Value="1"></asp:ListItem> <%--Case 0--%>
        <asp:ListItem Text="Parameters File" Value="2"></asp:ListItem> <%--Case 1--%>
        <asp:ListItem Text="Subarea File" Value="3"></asp:ListItem> <%--Case 2--%>
        <asp:ListItem Text="Soil File" Value="4"></asp:ListItem> <%--Case 3--%>
        <asp:ListItem Text="Layer File" Value="5"></asp:ListItem> <%--Case 4--%>
        <asp:ListItem Text="Operation File" Value="6"></asp:ListItem> <%--Case 5--%>
    </asp:DropDownList>

    <asp:Label ID="lblFields" Text="Field: " runat="server" ></asp:Label>
    <asp:DropDownList ID="ddlFields" runat="server" AutoPostBack="true"></asp:DropDownList>

    <asp:Label ID="lblSoils" Text="Select Soils" runat="server"></asp:Label>
    <asp:DropDownList ID="ddlSoils" runat="server" AutoPostBack="true"></asp:DropDownList>

    <asp:Label ID="lblScenarios" Text="Select Scenario: "  runat="server"></asp:Label>
    <asp:DropDownList ID="ddlScenarios" runat="server" AutoPostBack="true"></asp:DropDownList>

    <asp:LinkButton ID="btnSaveContinue" runat="server" Text="Save & Continue"></asp:LinkButton>
    
    <fieldset id="fsetControlfile" runat="server" class="section">
        <legend id="lblControl" runat="server">Control File</legend>
        <asp:GridView ID="gvControls" runat="server" AutoGenerateColumns="false">
            <FooterStyle CssClass="gvFooterStyle" />
            <RowStyle CssClass="gvRowStyle" />
            <HeaderStyle CssClass="gvHeaderStyle" />
                <Columns>
                 <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="true" ></asp:BoundField>
                 <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="true" ></asp:BoundField>
                 <asp:TemplateField HeaderText="Value1" >
                    <ItemTemplate>
                        <input id="txtValue1" value='<%#Eval("value1")%>' runat="server" class="gvTextBoxMedium" />
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:BoundField  DataField="range1" HeaderText="range1" ReadOnly="true" ItemStyle-CssClass="gvTextBoxMedium"></asp:BoundField >
                 <asp:BoundField  DataField="range2" HeaderText="range2" ReadOnly="true" ItemStyle-CssClass="gvTextBoxMedium"></asp:BoundField >
                </Columns>
        </asp:GridView>
    </fieldset>

    <fieldset id="fsetParamfile" runat="server" class="section">
        <legend id="lblParmFile" runat="server">Parametrs File</legend>
        <asp:GridView ID="gvParms" runat="server" AutoGenerateColumns="false"  >
            <FooterStyle CssClass="gvFooterStyle" />
            <RowStyle CssClass="gvRowStyle" />
            <HeaderStyle CssClass="gvHeaderStyle" />
                <Columns>
                <asp:BoundField  DataField="code" HeaderText="Code" ReadOnly="true" ></asp:BoundField>
                 <asp:BoundField  DataField="name" HeaderText="Name" ReadOnly="true" ></asp:BoundField>
                 <asp:TemplateField HeaderText="Value1" >
                    <ItemTemplate>
                        <asp:TextBox ID="txtValue1" Text='<%#Eval("value1")%>' runat="server" > </asp:TextBox>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:BoundField  DataField="range1" HeaderText="range1" ReadOnly="true" ></asp:BoundField>
                 <asp:BoundField  DataField="range2" HeaderText="range2" ReadOnly="true" ></asp:BoundField>
                </Columns>
        </asp:GridView>
    </fieldset>

    <fieldset ID="fsetSubareafile" runat="server" class="section">
        <legend id="lblSubarea" runat="server">Subarea File</legend>
        <asp:GridView ID="gvSubarea" runat="server" AutoGenerateColumns="false"  >
            <FooterStyle CssClass="gvFooterStyle" />
            <RowStyle CssClass="gvRowStyle" />
            <HeaderStyle CssClass="gvHeaderStyle" />
            <Columns>
                <asp:BoundField  DataField="sbatype" HeaderText="Type" ReadOnly="true" ></asp:BoundField>
                <asp:BoundField  DataField="subareanumber" HeaderText="#" ReadOnly="true" ></asp:BoundField>
                <asp:BoundField  DataField="subareatitle" HeaderText="Title" ReadOnly="true" ></asp:BoundField>
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <input type="button" id="btnShow" runat="server" value="Show" onclick="TurnTableOnOff();"/>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Lines Detail" >
                    <ItemTemplate>
                        <table id="tblLines" >
                            <tr>
                                <td>
                                    <asp:GridView ID="gvLine2" runat="server" AutoGenerateColumns="false">
                                        <HeaderStyle CssClass="gvHeaderStyle" />
                                        <Columns>
                                            <asp:BoundField DataField="LineNumber" HeaderText="Line #" ReadOnly="true" /> 
                                            <asp:TemplateField HeaderText="Inps">
                                                <ItemTemplate>
                                                    <input id="txtInps" type="text" value='<%#Eval("inps")%>' runat="server" class="gvTextBoxSmall"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Iops" >
                                                <ItemTemplate>
                                                    <input id="txtIops" value='<%#Eval("iops")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Iow" >
                                                <ItemTemplate>
                                                    <input id="txtIow" value='<%#Eval("iow")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ii" >
                                                <ItemTemplate>
                                                    <input id="txtIi" value='<%#Eval("Ii")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Iapl" >
                                                <ItemTemplate>
                                                    <input id="txtIapl" value='<%#Eval("iapl")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Nvcn" >
                                                <ItemTemplate>
                                                    <input id="txtNvcn" value='<%#Eval("nvcn")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Iwth" >
                                                <ItemTemplate>
                                                    <input id="txtIwth" value='<%#Eval("iwth")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ipts" >
                                                <ItemTemplate>
                                                    <input id="txtIpts" value='<%#Eval("Ipts")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Isao" >
                                                <ItemTemplate>
                                                    <input id="txtIsao" value='<%#Eval("isao")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Luns" >
                                                <ItemTemplate>
                                                    <input id="txtLuns" value='<%#Eval("luns")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Imw" >
                                                <ItemTemplate>
                                                    <input id="txtImw" value='<%#Eval("imw")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView id="gvLine3" runat="server" AutoGenerateColumns="false">
                                        <HeaderStyle CssClass="gvHeaderStyle" />
                                        <Columns>
                                            <asp:BoundField DataField="LineNumber" HeaderText="Line #" ReadOnly="true" /> 
                                             <asp:TemplateField HeaderText="Sno" >
                                                <ItemTemplate>
                                                    <input id="txtSno" value='<%#Eval("sno")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Stdo" >
                                                <ItemTemplate>
                                                    <input id="txtStdo" value='<%#Eval("stdo")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Yct" >
                                                <ItemTemplate>
                                                    <input id="txtYct" value='<%#Eval("yct")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Xct" >
                                                <ItemTemplate>
                                                    <input id="txtXct" value='<%#Eval("xct")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Azm" >
                                                <ItemTemplate>
                                                    <input id="txtAzm" value='<%#Eval("azm")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Fl" >
                                                <ItemTemplate>
                                                    <input id="txtFl" value='<%#Eval("fl")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Fw" >
                                                <ItemTemplate>
                                                    <input id="txtFw" value='<%#Eval("fw")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Angl" >
                                                <ItemTemplate>
                                                    <input id="txtAngl" value='<%#Eval("angl")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                        </Columns>                            
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView id="gvLine4" runat="server" AutoGenerateColumns="false">
                                        <HeaderStyle CssClass="gvHeaderStyle" />
                                        <Columns>
                                            <asp:BoundField DataField="LineNumber" HeaderText="Line #" ReadOnly="true" /> 
                                             <asp:TemplateField HeaderText="Wsa" >
                                                <ItemTemplate>
                                                    <input id="txtWas" value='<%#Eval("wsa")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="chl" >
                                                <ItemTemplate>
                                                    <input id="txtChl" value='<%#Eval("chl")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Chd" >
                                                <ItemTemplate>
                                                    <input id="txtChd" value='<%#Eval("chd")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Chs" >
                                                <ItemTemplate>
                                                    <input id="txtChs" value='<%#Eval("chs")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Chn" >
                                                <ItemTemplate>
                                                    <input id="txtChn" value='<%#Eval("chn")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Slp" >
                                                <ItemTemplate>
                                                    <input id="txtSlp" value='<%#Eval("slp")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Slpg" >
                                                <ItemTemplate>
                                                    <input id="txtSlpg" value='<%#Eval("slpg")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Upn" >
                                                <ItemTemplate>
                                                    <input id="txtUpn" value='<%#Eval("upn")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Ffpq" >
                                                <ItemTemplate>
                                                    <input id="txtFfpq" value='<%#Eval("ffpq")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Urnf" >
                                                <ItemTemplate>
                                                    <input id="txtUrnf" value='<%#Eval("urbf")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                        </Columns>                            
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView id="gvLine5" runat="server" AutoGenerateColumns="false">
                                        <HeaderStyle CssClass="gvHeaderStyle" />
                                        <Columns>
                                            <asp:BoundField DataField="LineNumber" HeaderText="Line #" ReadOnly="true" /> 
                                             <asp:TemplateField HeaderText="Rchl" >
                                                <ItemTemplate>
                                                    <input id="txtRch1" value='<%#Eval("rchl")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Rchd" >
                                                <ItemTemplate>
                                                    <input id="txtrchd" value='<%#Eval("rchd")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Rcbw" >
                                                <ItemTemplate>
                                                    <input id="txtRcbw" value='<%#Eval("rcbw")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Rctw" >
                                                <ItemTemplate>
                                                    <input id="txtrctw" value='<%#Eval("rctw")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Rchs" >
                                                <ItemTemplate>
                                                    <input id="txtrchs" value='<%#Eval("rchs")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Rchn" >
                                                <ItemTemplate>
                                                    <input id="txtrchn" value='<%#Eval("rchn")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Rchc" >
                                                <ItemTemplate>
                                                    <input id="txtrchc" value='<%#Eval("rchc")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Rchk" >
                                                <ItemTemplate>
                                                    <input id="txtrchk" value='<%#Eval("rchk")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Rfpw" >
                                                <ItemTemplate>
                                                    <input id="txtrfpw" value='<%#Eval("rfpw")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Rfpl" >
                                                <ItemTemplate>
                                                    <input id="txtrfpl" value='<%#Eval("rfpl")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                        </Columns>                            
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView id="gvLine6" runat="server" AutoGenerateColumns="false">
                                        <HeaderStyle CssClass="gvHeaderStyle" />
                                        <Columns>
                                            <asp:BoundField DataField="LineNumber" HeaderText="Line #" ReadOnly="true" /> 
                                             <asp:TemplateField HeaderText="Rsee" >
                                                <ItemTemplate>
                                                    <input id="txtrsee" value='<%#Eval("rsee")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Rsae" >
                                                <ItemTemplate>
                                                    <input id="txtrsae" value='<%#Eval("rsae")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Rsve" >
                                                <ItemTemplate>
                                                    <input id="txtrsve" value='<%#Eval("rsve")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Rsep" >
                                                <ItemTemplate>
                                                    <input id="txtrsep" value='<%#Eval("rsep")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Rsap" >
                                                <ItemTemplate>
                                                    <input id="txtrsap" value='<%#Eval("rsap")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Rsvp" >
                                                <ItemTemplate>
                                                    <input id="txtrsvp" value='<%#Eval("rsvp")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Rsv" >
                                                <ItemTemplate>
                                                    <input id="txtrsv" value='<%#Eval("rsv")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Rsrr" >
                                                <ItemTemplate>
                                                    <input id="txtrsrr" value='<%#Eval("rsrr")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Rsys" >
                                                <ItemTemplate>
                                                    <input id="txtrsys" value='<%#Eval("rsys")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Rsyn" >
                                                <ItemTemplate>
                                                    <input id="txtrsyn" value='<%#Eval("rsyn")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                        </Columns>                            
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView id="gvLine7" runat="server" AutoGenerateColumns="false">
                                        <HeaderStyle CssClass="gvHeaderStyle" />
                                        <Columns>
                                            <asp:BoundField DataField="LineNumber" HeaderText="Line #" ReadOnly="true" /> 
                                             <asp:TemplateField HeaderText="Rshc" >
                                                <ItemTemplate>
                                                    <input id="txtrshc" value='<%#Eval("rshc")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Rsdp" >
                                                <ItemTemplate>
                                                    <input id="txtrsdp" value='<%#Eval("rsdp")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Rsbd" >
                                                <ItemTemplate>
                                                    <input id="txtrsbd" value='<%#Eval("rsbd")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Pcof" >
                                                <ItemTemplate>
                                                    <input id="txtpcof" value='<%#Eval("pcof")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Bcof" >
                                                <ItemTemplate>
                                                    <input id="txtbcof" value='<%#Eval("bcof")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Bffl" >
                                                <ItemTemplate>
                                                    <input id="txtbffl" value='<%#Eval("bffl")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                         </Columns>                            
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView id="gvLine8" runat="server" AutoGenerateColumns="false">
                                        <HeaderStyle CssClass="gvHeaderStyle" />
                                        <Columns>
                                            <asp:BoundField DataField="LineNumber" HeaderText="Line #" ReadOnly="true" /> 
                                             <asp:TemplateField HeaderText="Nirr" >
                                                <ItemTemplate>
                                                    <input id="txtnirr" value='<%#Eval("nirr")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Iri" >
                                                <ItemTemplate>
                                                    <input id="txtiri" value='<%#Eval("iri")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Ifa" >
                                                <ItemTemplate>
                                                    <input id="txtifa" value='<%#Eval("ifa")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Lm" >
                                                <ItemTemplate>
                                                    <input id="txtlm" value='<%#Eval("lm")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Ifd" >
                                                <ItemTemplate>
                                                    <input id="txtifd" value='<%#Eval("ifd")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Idr" >
                                                <ItemTemplate>
                                                    <input id="txtidr" value='<%#Eval("idr")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Idf1" >
                                                <ItemTemplate>
                                                    <input id="txtidf1" value='<%#Eval("idf1")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Idf2" >
                                                <ItemTemplate>
                                                    <input id="txtidf2" value='<%#Eval("idf2")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Idf3" >
                                                <ItemTemplate>
                                                    <input id="txtidf3" value='<%#Eval("idf3")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Idf4" >
                                                <ItemTemplate>
                                                    <input id="txtidf4" value='<%#Eval("idf4")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Idf5" >
                                                <ItemTemplate>
                                                    <input id="txtidf5" value='<%#Eval("idf5")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                         </Columns>                            
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView id="gvLine9" runat="server" AutoGenerateColumns="false">
                                        <HeaderStyle CssClass="gvHeaderStyle" />
                                        <Columns>
                                            <asp:BoundField DataField="LineNumber" HeaderText="Line #" ReadOnly="true" /> 
                                             <asp:TemplateField HeaderText="Bir" >
                                                <ItemTemplate>
                                                    <input id="txtbir" value='<%#Eval("bir")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Efi" >
                                                <ItemTemplate>
                                                    <input id="txtefi" value='<%#Eval("efi")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Vimx" >
                                                <ItemTemplate>
                                                    <input id="txtvimx" value='<%#Eval("vimx")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Armn" >
                                                <ItemTemplate>
                                                    <input id="txtarmn" value='<%#Eval("armn")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Armx" >
                                                <ItemTemplate>
                                                    <input id="txtarmx" value='<%#Eval("armx")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Bft" >
                                                <ItemTemplate>
                                                    <input id="txtbft" value='<%#Eval("bft")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Fnp4" >
                                                <ItemTemplate>
                                                    <input id="txtfnp4" value='<%#Eval("fnp4")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Fmx" >
                                                <ItemTemplate>
                                                    <input id="txtfmx" value='<%#Eval("fmx")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Drt" >
                                                <ItemTemplate>
                                                    <input id="txtdrt" value='<%#Eval("drt")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Fdsf" >
                                                <ItemTemplate>
                                                    <input id="txtfdsf" value='<%#Eval("fdsf")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                        </Columns>                            
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView id="gvLine10" runat="server" AutoGenerateColumns="false">
                                        <HeaderStyle CssClass="gvHeaderStyle" />
                                        <Columns>
                                            <asp:BoundField DataField="LineNumber" HeaderText="Line #" ReadOnly="true" /> 
                                             <asp:TemplateField HeaderText="Pec" >
                                                <ItemTemplate>
                                                    <input id="txtpec" value='<%#Eval("pec")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Dalg" >
                                                <ItemTemplate>
                                                    <input id="txtdalg" value='<%#Eval("dalg")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Vlgn" >
                                                <ItemTemplate>
                                                    <input id="txtvlgn" value='<%#Eval("vlgn")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Coww" >
                                                <ItemTemplate>
                                                    <input id="txtcoww" value='<%#Eval("coww")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Ddlg" >
                                                <ItemTemplate>
                                                    <input id="txtddlg" value='<%#Eval("ddlg")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Solq" >
                                                <ItemTemplate>
                                                    <input id="txtsolq" value='<%#Eval("solq")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Sflg" >
                                                <ItemTemplate>
                                                    <input id="txtsflg" value='<%#Eval("sflg")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Fnp2" >
                                                <ItemTemplate>
                                                    <input id="txtfnp2" value='<%#Eval("fnp2")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Fnp5" >
                                                <ItemTemplate>
                                                    <input id="txtfnp5" value='<%#Eval("fnp5")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Firg" >
                                                <ItemTemplate>
                                                    <input id="txtfirg" value='<%#Eval("firg")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                        </Columns>                            
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView id="gvLine11" runat="server" AutoGenerateColumns="false">
                                        <HeaderStyle CssClass="gvHeaderStyle" />
                                        <Columns>
                                            <asp:BoundField DataField="LineNumber" HeaderText="Line #" ReadOnly="true" /> 
                                             <asp:TemplateField HeaderText="Ny1" >
                                                <ItemTemplate>
                                                    <input id="txtny1" value='<%#Eval("ny1")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Ny2" >
                                                <ItemTemplate>
                                                    <input id="txtny2" value='<%#Eval("ny2")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Ny3" >
                                                <ItemTemplate>
                                                    <input id="txtny3" value='<%#Eval("ny3")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Ny4" >
                                                <ItemTemplate>
                                                    <input id="txtny4" value='<%#Eval("ny4")%>' runat="server" class="gvTextBoxSmall" />
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                        </Columns>                            
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView id="gvLine12" runat="server" AutoGenerateColumns="false">
                                        <HeaderStyle CssClass="gvHeaderStyle" />
                                        <Columns>
                                            <asp:BoundField DataField="LineNumber" HeaderText="Line #" ReadOnly="true" /> 
                                            <asp:TemplateField HeaderText="Xtp1" >
                                            <ItemTemplate>
                                                <input id="txtxtp1" value='<%#Eval("xtp1")%>' runat="server" class="gvTextBoxSmall" />
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Xtp2" >
                                            <ItemTemplate>
                                                <input id="txtxtp2" value='<%#Eval("xtp2")%>' runat="server" class="gvTextBoxSmall" />
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Xtp3" >
                                            <ItemTemplate>
                                                <input id="txtxtp3" value='<%#Eval("xtp3")%>' runat="server" class="gvTextBoxSmall" />
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Xtp4" >
                                            <ItemTemplate>
                                                <input id="txtxtp4" value='<%#Eval("xtp4")%>' runat="server" class="gvTextBoxSmall" />
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>                               
            </Columns>
        </asp:GridView>
    </fieldset>

    <fieldset id="fsetSoilfile" runat="server" class="section">
        <legend id="lblSoil" runat="server">Soils File</legend>
        <asp:GridView id="gvSoils" runat="server" AutoGenerateColumns="false"  >
            <FooterStyle class="gvFooterStyle" />
            <RowStyle class="gvRowStyle" />
            <HeaderStyle CssClass="gvHeaderStyle" />
                <Columns>
                 <asp:BoundField  DataField="name" HeaderText="Name" ReadOnly="true" ></asp:BoundField>
                 <asp:BoundField  DataField="soilNumber" HeaderText="Soil #" ReadOnly="true" ItemStyle-class="gvTextBoxSmall"></asp:BoundField>
                 <asp:BoundField  DataField="key" HeaderText="Key" ReadOnly="true" ></asp:BoundField>
                <asp:TemplateField HeaderText="Albedo" >
                    <ItemTemplate>
                        <input id="txtAlbedo" value='<%#Eval("albedo")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Slope" >
                    <ItemTemplate>
                        <input id="txtSlope" value='<%#Eval("slope")%>' runat="server" class="gvTextBoxMedium"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Percentage" >
                    <ItemTemplate>
                        <input id="txtPercentage" value='<%#Eval("percentage")%>' runat="server" class="gvTextBoxMedium"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Ffc" >
                    <ItemTemplate>
                        <input id="txtFfc" value='<%#Eval("ffc")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Wtmn" >
                    <ItemTemplate>
                        <input id="txtWtmn" value='<%#Eval("wtmn")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Wtmx" >
                    <ItemTemplate>
                        <input id="txtWtmx" value='<%#Eval("wtmx")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Wtbl" >
                    <ItemTemplate>
                        <input id="txtWtbl" value='<%#Eval("wtbl")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Gwst" >
                    <ItemTemplate>
                        <input id="txtGwst" value='<%#Eval("gwst")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Gwmx" >
                    <ItemTemplate>
                        <input id="txtGwmx" value='<%#Eval("gwmx")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Rftt" >
                    <ItemTemplate>
                        <input id="txtRftt" value='<%#Eval("rftt")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Rfpk" >
                    <ItemTemplate>
                        <input id="txtRfpk" value='<%#Eval("rfpk")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Xids" >
                    <ItemTemplate>
                        <input id="txtXids" value='<%#Eval("xids")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Rtn1" >
                    <ItemTemplate>
                        <input id="txtRtn1" value='<%#Eval("rtn1")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Xidk" >
                    <ItemTemplate>
                        <input id="txtxidk" value='<%#Eval("xidk")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Zqt" >
                    <ItemTemplate>
                        <input id="txtZqt" value='<%#Eval("zqt")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Zf" >
                    <ItemTemplate>
                        <input id="txtZf" value='<%#Eval("zf")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Ztk" >
                    <ItemTemplate>
                        <input id="txtZtk" value='<%#Eval("ztk")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Fbm" >
                    <ItemTemplate>
                        <input id="txtFbm" value='<%#Eval("fbm")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Fhp" >
                    <ItemTemplate>
                        <input id="txtFhp" value='<%#Eval("fhp")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                </Columns>
        </asp:GridView>
    </fieldset>

    <fieldset id="fsetLayerfile" runat="server" class="section">
        <legend id="lblLayerFile" runat="server">Layer File</legend>
        <asp:GridView id="gvLayer" runat="server" AutoGenerateColumns="false"  >
            <FooterStyle class="gvFooterStyle" />
            <RowStyle class="gvRowStyle" />
            <HeaderStyle CssClass="gvHeaderStyle" />
                <Columns>
                 <asp:BoundField  DataField="LayerNumber" HeaderText="Layer Number" ReadOnly="true" ></asp:BoundField>
                 <asp:TemplateField HeaderText="Depth" >
                    <ItemTemplate>
                        <input id="txtDepth" value='<%#Eval("depth")%>' runat="server" class="gvTextBoxMedium"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="BD" >
                    <ItemTemplate>
                        <input id="txtBD" value='<%#Eval("bd")%>' runat="server" class="gvTextBoxMedium"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="UW" >
                    <ItemTemplate>
                        <input id="txtUW" value='<%#Eval("uw")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="FC" >
                    <ItemTemplate>
                        <input id="txtFC" value='<%#Eval("fc")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Sand" >
                    <ItemTemplate>
                        <input id="txtSand" value='<%#Eval("sand")%>' runat="server" class="gvTextBoxMedium"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Silt" >
                    <ItemTemplate>
                        <input id="txtSilt" value='<%#Eval("silt")%>' runat="server" class="gvTextBoxMedium"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="WN" >
                    <ItemTemplate>
                        <input id="txtWN" value='<%#Eval("wn")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="PH" >
                    <ItemTemplate>
                        <input id="txtPH" value='<%#Eval("ph")%>' runat="server" class="gvTextBoxMedium"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="SMB" >
                    <ItemTemplate>
                        <input id="txtSmb" value='<%#Eval("smb")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="WOC" >
                    <ItemTemplate>
                        <input id="txtOM" value='<%#Eval("om")%>' runat="server" class="gvTextBoxMedium"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="CAC" >
                    <ItemTemplate>
                        <input id="txtCac" value='<%#Eval("cac")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="CEC" >
                    <ItemTemplate>
                        <input id="txtCec" value='<%#Eval("cec")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="ROK" >
                    <ItemTemplate>
                        <input id="txtRok" value='<%#Eval("rok")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="CNDS" >
                    <ItemTemplate>
                        <input id="txtCnds" value='<%#Eval("cnds")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="SoilP/SSF" >
                    <ItemTemplate>
                        <input id="txtSoilP" value='<%#Eval("soilp")%>' runat="server" class="gvTextBoxMedium"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="RSD" >
                    <ItemTemplate>
                        <input id="txtRsd" value='<%#Eval("rsd")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="BDD" >
                    <ItemTemplate>
                        <input id="txtBDD" value='<%#Eval("bdd")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="PSP" >
                    <ItemTemplate>
                        <input id="txtPsp" value='<%#Eval("psp")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="SATC" >
                    <ItemTemplate>
                        <input id="txtSatc" value='<%#Eval("satc")%>' runat="server" class="gvTextBoxSmall"/>
                    </ItemTemplate>
                 </asp:TemplateField>
                </Columns>
        </asp:GridView>
    </fieldset>

    <fieldset id="fsetOperationfile" runat="server" class="section">
        <legend id="lblOperation" runat="server">Operation File</legend>
        <asp:GridView id="gvOperation" runat="server" AutoGenerateColumns="false"  >
            <FooterStyle class="gvFooterStyle" />
            <RowStyle class="gvRowStyle" />
            <HeaderStyle CssClass="gvHeaderStyle" />
                <Columns>
                <asp:TemplateField HeaderText="Year" >
                    <ItemTemplate>
                        <input id="txtYear" value='<%#Eval("year")%>' runat="server" class="gvTextBoxSmall" />
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Month" >
                    <ItemTemplate>
                        <input id="txtMonth" value='<%#Eval("month")%>' runat="server" class="gvTextBoxSmall" />
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Day" >
                    <ItemTemplate>
                        <input id="txtDay" value='<%#Eval("day")%>' runat="server" class="gvTextBoxSmall" />
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Operation" >
                    <ItemTemplate>
                        <input id="txtApexOp" value='<%#Eval("apexTillCode")%>' runat="server" class="gvTextBoxMedium" />
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Tractor id" >
                    <ItemTemplate>
                        <input id="txtTractorId" value='<%#Eval("tractorid")%>' runat="server" class="gvTextBoxSmall" />
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Crop" >
                    <ItemTemplate>
                        <input id="txtApexCrop" value='<%#Eval("apexcrop")%>' runat="server" class="gvTextBoxSmall" />
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="JX(7)/id" >
                    <ItemTemplate>
                        <input id="txtApexFert" value='<%#Eval("apexoptype")%>' runat="server" class="gvTextBoxSmall" />
                    </ItemTemplate>
                 </asp:TemplateField>
                <asp:TemplateField HeaderText="OPV1" >
                    <ItemTemplate>
                        <input id="txtOpVal1" value='<%#Eval("opval1")%>' runat="server" class="gvTextBox" />
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="OPV2" >
                    <ItemTemplate>
                        <input id="txtOpVal2" value='<%#Eval("opval2")%>' runat="server" class="gvTextBoxMedium" />
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="OPV3" >
                    <ItemTemplate>
                        <input id="txtOpVal3" value='<%#Eval("opval3")%>' runat="server" class="gvTextBoxMedium" />
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="OPV4" >
                    <ItemTemplate>
                        <input id="txtOpVal4" value='<%#Eval("opval4")%>' runat="server" class="gvTextBoxMedium" />
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="OPV5" >
                    <ItemTemplate>
                        <input id="txtOpVal5" value='<%#Eval("opval5")%>' runat="server" class="gvTextBoxMedium" />
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="OPV6" >
                    <ItemTemplate>
                        <input id="txtOpVal6" value='<%#Eval("opval6")%>' runat="server" class="gvTextBoxMedium" />
                    </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="OPV7" >
                    <ItemTemplate>
                        <input id="txtOpVal7" value='<%#Eval("opval7")%>' runat="server" class="gvTextBoxMedium" />
                    </ItemTemplate>
                 </asp:TemplateField>
                </Columns>
        </asp:GridView>
    </fieldset>
    <%--Start of Buttons--%>
        
</asp:Content>