<%@ Page Language="C#" MasterPageFile="~masterurl/default.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ID="title" ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	Human Resources - Home
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	Human Resources - Home
</asp:Content>

<asp:Content ID="main" ContentPlaceHolderId="PlaceHolderMain" runat="server">
	<div style="padding: 25px">
		<p>This is my awesome Human Resources homepage!</p>
		<p>We might have some links to other HR-related pages here.</p>
		<ul>
			<li><a href="HRPage2.aspx">HR Page 2</a></li>
			<li><a href="HRPage3.aspx">HR Page 3</a></li>
		</ul>
		<p>We might have some links to HR-related lists below.</p>
		<ul>
			<li><a href="/Lists/Employees">Employees</a></li>
		</ul>
	</div>
</asp:Content>