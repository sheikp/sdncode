<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerDetails.aspx.cs" Inherits="SDNPortal._CustomerDetails" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-wrapper">
        <section id="main" class="container">								
            <div class="row">
						<div class="12u">
                            <section class="box">
                                
					                <div class="row uniform 50%">
                                        <div class="4u 12u(mobilep)">
												<h4>Customer Number</h4>
											</div>
											<div class="4u 12u(mobilep)">
												<h4>Customer Name</h4>
											</div>
                                            <div class="4u 12u(mobilep)">
												<h4>Location</h4>
											</div>											
										</div>  
                                <div class="row uniform 50%">
                                        <div class="4u 12u(mobilep)">
												<input runat="server" type="text" name="query" id="txtNumber" value="" placeholder="Customer Number" readonly="readonly" />
											</div>
											<div class="4u 12u(mobilep)">
												<input runat="server" type="text" name="query" id="txtName" value="" placeholder="Customer Name" readonly="readonly" />
											</div>
                                            <div class="4u 12u(mobilep)">
												<input runat="server" type="text" name="query" id="txtLocation" value="" placeholder="Location" readonly="readonly" />
											</div>											
										</div>  
                                <div class="row uniform 50%">
                                        <div class="6u">
												<h4>List of Routers &nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnkRouterAdd" runat="server" OnClick="lnkRouterAdd_Click" Visible="false">Add</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnkEditRout" OnClick="lnkEditRout_Click" runat="server" Visible="false"><span style="border:1px solid red;padding:5px; background:#e3e3e3">Change Policy Mapping</span></asp:LinkButton></h4>
											</div>                                 
                                        <div class="6u">
												<h4>Policies on selected Router &nbsp;&nbsp;<asp:LinkButton ID="lnkModify" runat="server" OnClick="lnkModify_Click"><span style="color:#ffffff;font-size:medium; padding:5px; background:#666">View / Modify</span></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton ID="lnkPolicyAdd" runat="server" OnClick="lnkPolicyAdd_Click"><span style="color:#ffffff; font-size:medium ;padding:5px; background:#666">Add Policy</span></asp:LinkButton></h4>
										</div>            
                                     </div>        
                                <div class="row uniform 50%">
                                        <div class="6u">
												<div class="select-wrapper">
													<asp:ListBox ID="lstRouters" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstRouters_SelectedIndexChanged"></asp:ListBox>
												</div>
											</div>
                                 
                                    <div class="6u">
												<div class="select-wrapper">
													<asp:ListBox ID="lstPolicy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstPolicy_SelectedIndexChanged"></asp:ListBox>
												</div>
											</div>            
                                     </div>         
                                <div class="row uniform 50%" style="display:none">
                                        <div class="12u">
												<div class="select-wrapper">
													<h4>Policy Information</h4>
												</div>
											</div>
                                     </div>  
                                 <div class="row uniform 50%" style="display:none">
                                        <div class="12u">
												<div class="select-wrapper">
													<textarea name="message" id="description" placeholder="Policy Description" rows="6" runat="server"></textarea>
												</div>
											</div>
                                     </div> 
                                 <div class="row uniform">
						                <div class="12u">
							                <ul class="actions">
								                <li><asp:Button runat="server" ID="btnSave" Text="Change Policy Mapping" OnClick="lnkEditRout_Click" /></li>
								                <li><asp:Button runat="server" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" CssClass="alt" /></li>    
							                </ul>
						                </div>
					                </div>
                                </section>
						</div>
					</div>                  
                           
		</section>
    </div>
    <asp:Label ID="lblHidden" runat="server" Text=""></asp:Label>
    <ajaxToolkit:ModalPopupExtender ID="mpePopUp" runat="server" TargetControlID="lblHidden" PopupControlID="divRoutPopUp" BackgroundCssClass="popubackground"></ajaxToolkit:ModalPopupExtender>

   
    <div id="divRoutPopUp"class="box" style="width:800px">
     <h3>Policy Mapping</h3>   
        <asp:Label ID="lblpopRmessage" runat="server"></asp:Label>
     <div class="row">
						<div class="12u">
                            <section class="box">                                
					                
                                <div class="row uniform 50%">                                  
                                        <div class="12u 12u(mobilep)">
                                             <input type="hidden" id="hidRType" runat="server" />
												<textarea class="fit" runat="server" name="query" id="txtRouterName" rows="1" value="" placeholder="Router Name" />
											</div>											
										</div>  
                                                              
                                 <div class="row uniform 50%"">
                                        <div class="12u">
												<ul class="actions">
													<li class="5u"><h4>Inventory of Policies</h4></li>
                                                    <li class="2u">&nbsp;</li>
                                                    <li class="4u"><h4>Policies in Router</h4></li>
                                                    </ul>
												</div>
											</div>
                                 <div class="row uniform 50%"">
                                        <div class="12u">
												<ul class="actions">
													<li class="5u"><asp:ListBox ID="lstPavbl" runat="server" SelectionMode="Multiple"  Rows="6" ></asp:ListBox></li>
                                                    <li class="2u"><asp:Button runat="server" CssClass="button special small" ID="PMap" OnClick="PMap_Click" Text=">>" /><br /><br /><asp:Button CssClass="button special small" runat="server" ID="PuMap" OnClick="PuMap_Click" Text="<<" /></li>
                                                    <li class="4u"><asp:ListBox ID="lstPmap" runat="server"  Rows="6"  ></asp:ListBox></li>
                                                    </ul>
												</div>
											</div>
                                 
                                 <div class="row uniform 50%">                                  
                                    
                                     <div class="12u">
                                         <ul class="actions">
                                              <li class="5u"><h4>Selected Policies</h4></li>
                                             <li class="2u">&nbsp;</li> 
												<li class="4u"><h4>Router Interfaces</h4></li>
                                             
                                             </ul>
											</div>											
										</div>
                                   										
                                    <div class="12u">
                                         <ul class="actions">
                                              <li class="5u"><asp:ListBox ID="lstPmap1" runat="server"  ></asp:ListBox></li>
                                             <li><asp:Button runat="server" ID="routMapInterface" Text="Map" OnClick="routMapInterface_Click"  CssClass="button special small" /></li> 
												<li class="4u"><asp:ListBox ID="lstInterfaces" runat="server"></asp:ListBox></li>
                                             
                                             </ul>
											</div>											
										
                                

                                 <div class="row uniform">
						                <div class="12u">
							                <ul class="actions">
								                <li><asp:Button runat="server" ID="routSave" Text="Update" OnClick="routSave_Click"/></li>
                                                                                                                                               
								                <li><asp:Button runat="server" ID="routCancel" Text="Cancel" OnClick="routCancel_Click" CssClass="alt" /></li>                                                                                                
							                </ul>
						                </div>
					                </div>
                                </section>
						</div>
         </div>
					</div> 
    
    

    <asp:Label ID="lblHid2" runat="server" Text=""></asp:Label>
    <ajaxToolkit:ModalPopupExtender ID="mpePopUp2" runat="server" TargetControlID="lblHid2" PopupControlID="divPolyPopUp" BackgroundCssClass="popubackground"></ajaxToolkit:ModalPopupExtender>

   
    <div id="divPolyPopUp"class="box" style="width:800px">
     <h3>Policy Update</h3>     
     <div class="row">
						<div class="12u">
                            <section class="box">                                
					                
                                <div class="row uniform 50%">                                  
                                        <div class="12u 12u(mobilep)">
												<textarea class="fit" runat="server" name="query" id="txtNewPloicyName" rows="1" value="" placeholder="Policy Nuame" />
											</div>											
										</div>  

                                <div class="row uniform 50%">                                  
                                        <div class="12u 12u(mobilep)">
												<textarea class="fit" runat="server" name="query" id="txtNewPolicyDescr" rows="4" value="" placeholder="Policy Detail" />
                                                <input type="hidden" id="hidimpact" runat="server" />
											</div>											
										</div> 
                                 
                                 <div class="row uniform">
						                <div class="12u">
							                <ul class="actions">
								                <li><asp:Button runat="server" ID="polySave" Text="Save Policy Detail" OnClick="polySave_Click"/></li>
								                <li><asp:Button runat="server" ID="polyCancel" Text="Cancel" CssClass="alt" OnClick="polyCancel_Click" /></li>                                                                                                
							                </ul>
						                </div>
					                </div>
                                </section>
						</div>
					</div> 
    </div>

    <asp:Label ID="lblHid3" runat="server" Text=""></asp:Label>
    <ajaxToolkit:ModalPopupExtender ID="mpePopUp3" runat="server" TargetControlID="lblHid3" PopupControlID="divUpdatePopUp" BackgroundCssClass="popubackground"></ajaxToolkit:ModalPopupExtender>

   
    <div id="divUpdatePopUp"class="box" style="width:800px">
     <h3>Impacted Routers / Customers</h3>     
     <div class="row">
						<div class="12u">
                            <section class="box">                                
					                
                                <div class="table-wrapper">
                                        <asp:GridView ID="CustomersGrid" runat="server" AutoGenerateColumns="False"                                             
                                            EmptyDataText="There are no data records to display.">
                                            <Columns>
                                                <asp:TemplateField>                                                    
                                                    <HeaderTemplate>
                                                        <asp:CheckBox id="chkselh" runat="server" Text="&nbsp;" OnCheckedChanged="chkselh_CheckedChanged" AutoPostBack="true"/>                                                        
                                                    </HeaderTemplate>
                                                    <ItemTemplate>                                                        
                                                        <asp:CheckBox id="chksel" runat="server" Text="&nbsp;"/>                                                        
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="router_id" Visible="false" />     
                                                
                                                <asp:BoundField DataField="routersname" HeaderText="Router Name" 
                                                    SortExpression="routername" />
                                                                                                                                  
                                           
                                            </Columns>
                                        </asp:GridView>                                        
										
									</div>
                                 
                                 <div class="row uniform">
						                <div class="12u">
							                <ul class="actions">
								                <li><asp:Button runat="server" ID="btnUpdatePolicy" Text="Update Policy Detail" OnClick="btnUpdatePolicy_Click" /></li>
								                <li><asp:Button runat="server" ID="btnCancelP" Text="Cancel" CssClass="alt" OnClick="btnCancelP_Click"/></li>                                                                                                
							                </ul>
						                </div>
					                </div>
                                </section>
						</div>
					</div> 
    </div>

</asp:Content>
