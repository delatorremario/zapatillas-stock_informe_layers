<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/alertify.min.js"></script>
</head>
<body>

    

    <div class="container">
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
        <h3>Consultar Stock en Sucursales</h3>
            <div>
                <asp:RadioButtonList ID="rbConexiones" runat="server"></asp:RadioButtonList>
            </div>
            <div>
                <h4>Seleccionar el archivo con los códigos</h4>
                <div class="row">
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                </div>
                <div class="row">
                    <asp:Button ID="UploadButton" runat="server" Text="Subir" 
                            UseSubmitBehavior="False" OnClick="UploadButton_Click" />
                </div>

            </div>
            <div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>

                                <div class="resultadoExcel">
                                    <div class="row">
                                        <asp:Label ID="UploadStatusLabel" runat="server"></asp:Label>
                                    </div>
                                    <div class="row">
                                        <asp:Button CssClass="btn btn-default" ID="btProcesar" runat="server" Text="Procesar" UseSubmitBehavior="False" OnClick="btProcesar_Click" />
                                    </div>

                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                                                     AssociatedUpdatePanelID="UpdatePanel1">
                                        <ProgressTemplate>
                                                 Actualización en Progreso ....
                                            <img src="../../../progreso.gif" align="middle" alt="."/>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <div>
                                        <asp:GridView ID="GridView1" runat="server">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbMensajeRow" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                    </asp:GridView>
                                    </div>
                                </div>

                                
                            </ContentTemplate>
                        </asp:UpdatePanel>
            </div>
        </div>
        </form>
        
    </div>
    
</body>
</html>
