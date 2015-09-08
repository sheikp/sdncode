<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SDNPortal._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-wrapper" >
        <section id="main" class="container" >
					
            <img src="img/banner1.jpg" width="100%" />			
            <div class="row">
						<div class="12u">
                            <section class="box">
                                
					                <div class="row uniform 50%">
                                        <div class="3u 12u(mobilep)">
												<input runat="server" type="text" name="query" id="txtNumber" value="" placeholder="Customer Number" />
											</div>
											<div class="3u 12u(mobilep)">
												<input runat="server" type="text" name="query" id="txtName" value="" placeholder="Customer Name" />
											</div>
                                            <div class="3u 12u(mobilep)">
												<input runat="server" type="text" name="query" id="txtLocation" value="" placeholder="Location" />
											</div>
											<div class="3u 12u(mobilep)">
												<asp:Button Text="Search" CssClass="fit" runat="server" OnClick="Search_Click" />
											</div>
										</div>                                
                                </section>
						</div>
					</div>
            
                           <div style="display:none" id="custitle" runat="server"> <h4>Customers</h4> </div>
									<div class="table-wrapper">
                                        <asp:GridView ID="CustomersGrid" runat="server" AutoGenerateColumns="False"                                             
                                            EmptyDataText="There are no data records to display.">
                                            <Columns>
                                                <asp:hyperlinkfield headertext="Customer Number" DataNavigateUrlFields="customernumber"         
                                                      DataTextField="customernumber"                                                      
                                                      datanavigateurlformatstring="~\customerdetails.aspx?CustomerNumber={0}" />                                                
                                            </Columns>
                                            <Columns>
                                                <asp:BoundField DataField="customername" HeaderText="Customer Name" 
                                                    SortExpression="customername" />
                                            </Columns>
                                            <Columns>
                                                <asp:BoundField DataField="location" HeaderText="Location" 
                                                    SortExpression="location" />
                                            </Columns>
                                        </asp:GridView>                                        
										
									</div>
		</section>
    </div>

</asp:Content>
