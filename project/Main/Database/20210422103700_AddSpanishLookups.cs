namespace Main.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20210422103700)]
	public class AddSpanishLookups : Migration
	{
		public override void Up()
		{
			string tableName = "[LU].[AddressType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'Trabajo', 'es', '1', 0, 0");
				InsertLookupValue(tableName, columns, "'Facturación', 'es', '2', 0, 1");
				InsertLookupValue(tableName, columns, "'Entrega', 'es', '3', 0, 2");
				InsertLookupValue(tableName, columns, "'Privada', 'es', '4', 0, 3");
			}
			tableName = "[LU].[BravoCategory]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value" };
				InsertLookupValue(tableName, columns, "'Problemas principales', 'es', '100'");
				InsertLookupValue(tableName, columns, "'Otros', 'es', '101'");
			}
			tableName = "[LU].[BusinessRelationshipType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "InverseRelationshipTypeKey" };
				InsertLookupValue(tableName, columns, "'Competidor', 'es', '102', '102'");
				InsertLookupValue(tableName, columns, "'Socio', 'es', '103', '103'");
				InsertLookupValue(tableName, columns, "'Proveedor', 'es', '101', '100'");
				InsertLookupValue(tableName, columns, "'Cliente', 'es', '100', '101'");
			}
			tableName = "[LU].[CommunicationType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "CommTypeGroupKey", "Language", "Value", "SortOrder" };
				InsertLookupValue(tableName, columns, "'Otro', 'Website', 'es', 'WebsiteOther', 2", false);
				InsertLookupValue(tableName, columns, "'Personal', 'Website', 'es', 'WebsitePersonal', 1", false);
				InsertLookupValue(tableName, columns, "'Work', 'Website', 'es', 'WebsiteWork', 0", false);
				InsertLookupValue(tableName, columns, "'Otro', 'Email', 'es', 'EmailOther', 2", false);
				InsertLookupValue(tableName, columns, "'Personal', 'Email', 'es', 'EmailPersonal', 1", false);
				InsertLookupValue(tableName, columns, "'Work', 'Email', 'es', 'EmailWork', 0", false);
				InsertLookupValue(tableName, columns, "'Otro', 'Fax', 'es', 'FaxOther', 2", false);
				InsertLookupValue(tableName, columns, "'Personal', 'Fax', 'es', 'FaxPersonal', 1", false);
				InsertLookupValue(tableName, columns, "'Work', 'Fax', 'es', 'FaxWork', 0", false);
				InsertLookupValue(tableName, columns, "'Otro', 'Phone', 'es', 'PhoneOther', 5", false);
				InsertLookupValue(tableName, columns, "'Recepción', 'Phone', 'es', 'PhoneFront', 0", false);
				InsertLookupValue(tableName, columns, "'Fijo', 'Phone', 'es', 'PhoneHome', 3", false);
				InsertLookupValue(tableName, columns, "'Localizador', 'Phone', 'es', 'PhonePager', 4", false);
				InsertLookupValue(tableName, columns, "'Móvil', 'Phone', 'es', 'PhoneMobile', 2", false);
				InsertLookupValue(tableName, columns, "'Trabajo', 'Phone', 'es', 'PhoneWork', 1", false);
			}
			tableName = "[LU].[CompanyType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'Socio', 'es', '102', 0, 0");
				InsertLookupValue(tableName, columns, "'Competidor', 'es', '103', 0, 0");
				InsertLookupValue(tableName, columns, "'Cliente', 'es', '101', 0, 0");
				InsertLookupValue(tableName, columns, "'Interesado', 'es', '100', 0, 0");
			}
			tableName = "[LU].[Currency]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "SortOrder" };
				InsertLookupValue(tableName, columns, "'€', 'es', 'EUR', 0");
				InsertLookupValue(tableName, columns, "'$', 'es', 'USD', 1");
				InsertLookupValue(tableName, columns, "'£', 'es', 'GBP', 2");
			}
			tableName = "[LU].[EmailType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'Otro', 'es', 'EmailOther', 0, 2");
				InsertLookupValue(tableName, columns, "'Casa', 'es', 'EmailHome', 0, 1");
				InsertLookupValue(tableName, columns, "'Trabajo', 'es', 'EmailWork', 0, 0");
			}
			tableName = "[LU].[FaxType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'Otro', 'es', 'FaxOther', 0, 2");
				InsertLookupValue(tableName, columns, "'Home', 'es', 'FaxHome', 0, 1");
				InsertLookupValue(tableName, columns, "'Trabajo', 'es', 'FaxWork', 0, 0");
			}
			tableName = "[LU].[InvoicingType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'Goodwill', 'es', 'Goodwill', 0, 2");
				InsertLookupValue(tableName, columns, "'Suma agrupada', 'es', 'LumpSum', 0, 0");
				InsertLookupValue(tableName, columns, "'Por Estado', 'es', 'ByStatus', 0, 2");
				InsertLookupValue(tableName, columns, "'Base de T&M', 'es', 'TMbasis', 0, 1");
			}
			tableName = "[LU].[NoCausingItemPreviousSerialNoReason]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'No legible', 'es', 'Nonreadable', 0, 0");
				InsertLookupValue(tableName, columns, "'No disponible', 'es', 'NotAvailable', 0, 0");
			}
			tableName = "[LU].[NoCausingItemSerialNoReason]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'No sustituido', 'es', 'NotReplaced', 0, 0");
				InsertLookupValue(tableName, columns, "'No disponible', 'es', 'NotAvailable', 0, 0");
			}
			tableName = "[LU].[NoPreviousSerialNoReason]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'No legible', 'es', 'Nonreadable', 0, 1");
				InsertLookupValue(tableName, columns, "'No disponible', 'es', 'NotAvailable', 0, 0");
			}
			tableName = "[LU].[NoteType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Color", "Icon", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'Caso de servicio creado', 'es', 'ServiceCaseCreatedNote', '#4caf50', '\\f1ed', 0, 0");
				InsertLookupValue(tableName, columns, "'Trabajo completado', 'es', 'ServiceOrderDispatchCompletedNote', '#8BC34A', '\\f1ed', 0, 0");
				InsertLookupValue(tableName, columns, "'Temas del informe de la visita', 'es', 'VisitReportTopicNote', '#4caf50', '\\f196', 0, 0");
				InsertLookupValue(tableName, columns, "'Informe de la visita completado', 'es', 'VisitReportClosedNote', '#9164a6', '\\f196', 0, 0");
				InsertLookupValue(tableName, columns, "'Pedido de servicio creado', 'es', 'ServiceOrderHeadCreatedNote', '#4caf50', '\\f156', 0, 0");
				InsertLookupValue(tableName, columns, "'Cambio de estado del caso de servicio', 'es', 'ServiceCaseStatusChangedNote', '#9164a6', '\\f12d', 0, 0");
				InsertLookupValue(tableName, columns, "'Se ha modificado el estado del proyecto', 'es', 'ProjectStatusChangedNote', '#9164a6', '\\f223', 0, 0");
				InsertLookupValue(tableName, columns, "'Proyecto perdido', 'es', 'ProjectLostNote', '#ff1843', '\\f223', 0, 0");
				InsertLookupValue(tableName, columns, "'Nuevo proyecto', 'es', 'ProjectCreatedNote', '#4caf50', '\\f223', 0, 0");
				InsertLookupValue(tableName, columns, "'Se ha modificado el estado de la orden', 'es', 'OrderStatusChangedNote', '#9164a6', '\\f156', 0, 0");
				InsertLookupValue(tableName, columns, "'Se ha modificado el estado de la orden', 'es', 'BaseOrderStatusChangedNote', '#2196F3', '\\f19a', 0, 0");
				InsertLookupValue(tableName, columns, "'Nueva orden', 'es', 'BaseOrderCreatedNote', '#4caf50', '\\f19a', 0, 0");
				InsertLookupValue(tableName, columns, "'Tarea completada', 'es', 'TaskCompletedNote', '#4caf50', '\\f108', 0, 0");
				InsertLookupValue(tableName, columns, "'Nota del usuario', 'es', 'UserNote', '#2196f3', '\\f25b', 0, 0");
				InsertLookupValue(tableName, columns, "'Email', 'es', 'EmailNote', '#2196F3', '\\f15a', 0, 0");
				InsertLookupValue(tableName, columns, "'Llamada', 'es', 'CallEndedNote', '#edde55', '\\f2be', 0, 0");
			}
			tableName = "[LU].[NumberOfEmployees]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'500 - 1''000', 'es', '105', 0, 5");
				InsertLookupValue(tableName, columns, "'250 - 499', 'es', '104', 0, 5");
				InsertLookupValue(tableName, columns, "'Más de 1''000', 'es', '106', 0, 6");
				InsertLookupValue(tableName, columns, "'100 - 249', 'es', '103', 0, 3");
				InsertLookupValue(tableName, columns, "'25 - 99', 'es', '102', 0, 2");
				InsertLookupValue(tableName, columns, "'Menos de 25', 'es', '101', 0, 1");
				InsertLookupValue(tableName, columns, "'No se aplica', 'es', '100', 0, 0");
			}
			tableName = "[LU].[PaymentInterval]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Value", "Name", "Language", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "2, 'Trimestralmente', 'es', 0, 1");
				InsertLookupValue(tableName, columns, "1, 'Inmediatamente', 'es', 0, 0");
			}
			tableName = "[LU].[PaymentType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Value", "Name", "Language", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "2, 'Débito', 'es', 0, 1");
				InsertLookupValue(tableName, columns, "1, 'Factura', 'es', 0, 0");
			}
			tableName = "[LU].[PaymentCondition]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Value", "Name", "Language", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'1', 'A pagar en el momento de la recepción sin descuento', 'es', 0, 0");
			}
			tableName = "[LU].[PhoneType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'Trabajo', 'es', 'PhoneWork', 0, 1");
				InsertLookupValue(tableName, columns, "'Móvil', 'es', 'PhoneMobile', 0, 2");
				InsertLookupValue(tableName, columns, "'Fijo', 'es', 'PhoneHome', 0, 3");
				InsertLookupValue(tableName, columns, "'Recepción', 'es', 'PhoneFront', 0, 0");
				InsertLookupValue(tableName, columns, "'Localizador', 'es', 'PhonePager', 0, 4");
				InsertLookupValue(tableName, columns, "'Otro', 'es', 'PhoneOther', 0, 5");
			}
			tableName = "[LU].[Region]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Language", "Name", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'es', 'Otro', '100', 0, 0");
			}
			tableName = "[LU].[ReplicationGroup]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Value", "Name", "Language", "DefaultValue", "Description", "HasParameter", "TableName", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'NoteHistory', 'Historia de la nota', 'es', '120', 'Notas de los últimos x días. 0 para sincronizar todo el historial.', 1, 'Crm_Note', 0, 0");
			}
			tableName = "[LU].[Salutation]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'100', 'es', '100', 0, 0");
				InsertLookupValue(tableName, columns, "'Sr.', 'es', '101', 0, 0");
				InsertLookupValue(tableName, columns, "'Sra.', 'es', '102', 0, 0");
			}
			tableName = "[LU].[SalutationLetter]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'100', 'es', '100', 0, 0");
				InsertLookupValue(tableName, columns, "'Estimado señor', 'es', '101', 0, 0");
				InsertLookupValue(tableName, columns, "'Estimada señora', 'es', '102', 0, 0");
			}
			tableName = "[LU].[SourceType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "CampaignType", "Favorite", "SortOrder", "Color" };
				InsertLookupValue(tableName, columns, "'Otro', 'es', '100', 1, 0, 0, '#9E9E9E'");
				InsertLookupValue(tableName, columns, "'Eventos', 'es', '101', 1, 0, 0, '#9E9E9E'");
				InsertLookupValue(tableName, columns, "'Marketing online', 'es', '102', 1, 0, 0, '#9E9E9E'");
				InsertLookupValue(tableName, columns, "'Socio', 'es', '103', 0, 0, 0, '#9E9E9E'");
				InsertLookupValue(tableName, columns, "'Adquisición de clientes', 'es', '104', 0, 0, 0, '#9E9E9E'");
				InsertLookupValue(tableName, columns, "'Recomendación', 'es', '105', 0, 0, 0, '#9E9E9E'");
				InsertLookupValue(tableName, columns, "'Correo electrónico', 'es', '106', 1, 0, 0, '#9E9E9E'");
			}
			tableName = "[LU].[TaskType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder", "Color" };
				InsertLookupValue(tableName, columns, "'Ninguno', 'es', '100', 0, 0, '#AAAAAA'");
				InsertLookupValue(tableName, columns, "'Llamar a', 'es', '101', 0, 0, '#AAAAAA'");
				InsertLookupValue(tableName, columns, "'Demo', 'es', '102', 0, 0, '#AAAAAA'");
				InsertLookupValue(tableName, columns, "'Fax', 'es', '103', 0, 0, '#AAAAAA'");
				InsertLookupValue(tableName, columns, "'Seguimiento', 'es', '104', 0, 0, '#AAAAAA'");
				InsertLookupValue(tableName, columns, "'Reunión', 'es', '105', 0, 0, '#AAAAAA'");
			}
			tableName = "[LU].[Title]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'Ninguno', 'es', '100', 0, 0");
				InsertLookupValue(tableName, columns, "'Dr.', 'es', '101', 0, 0");
				InsertLookupValue(tableName, columns, "'Prof.', 'es', '102', 0, 0");
			}
			tableName = "[LU].[WebsiteType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'Trabajo', 'es', 'WebsiteWork', 0, 0");
				InsertLookupValue(tableName, columns, "'Personal', 'es', 'WebsitePersonal', 0, 1");
				InsertLookupValue(tableName, columns, "'Otro', 'es', 'WebsiteOther', 0, 2");
			}
			tableName = "[LU].[TimeUnit]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder", "TimeUnitsPerYear" };
				InsertLookupValue(tableName, columns, "'Años', 'es', 'Year', 0, 10, 1");
				InsertLookupValue(tableName, columns, "'Meses', 'es', 'Month', 0, 30, 12");
				InsertLookupValue(tableName, columns, "'Semanas', 'es', 'Week', 0, 40, NULL"); 
				InsertLookupValue(tableName, columns, "'Dias', 'es', 'Day', 0, 50, NULL");
				InsertLookupValue(tableName, columns, "'Horas', 'es', 'Hour', 1, 60, NULL");
				InsertLookupValue(tableName, columns, "'Minutos', 'es', 'Minute', 0, 70, NULL");
				InsertLookupValue(tableName, columns, "'Segundos', 'es', 'Second', 0, 80, NULL");
				InsertLookupValue(tableName, columns, "'Milisegundos', 'es', 'Millisecond', 0, 90, NULL");
				InsertLookupValue(tableName, columns, "'Trimestre', 'es', 'Quarter', 0, 20, 4");
			}
			tableName = "[LU].[CompanyGroupFlag1]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'Fabricación de vehículos', 'es', 'VehicleManufacturing', 0, 0");
				InsertLookupValue(tableName, columns, "'Limpieza', 'es', 'CommercialCleaning', 0, 0");
				InsertLookupValue(tableName, columns, "'Asistencia sanitaria', 'es', 'HealthCare', 0, 0");
			}
			tableName = "[LU].[DocumentCategory]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Value", "Name", "Language" };
				InsertLookupValue(tableName, columns, "'Document', 'Documento', 'es'");
			}
			tableName = "[LU].[Country]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder", "Iso2Code", "Iso3Code", "CallingCode" };
				InsertLookupValue(tableName, columns, "'Afganistán', 'es', 101, 0, 0, 'AF', 'AFG', 93");
				InsertLookupValue(tableName, columns, "'Albania', 'es', 102, 0, 0, 'AL', 'ALB', 355");
				InsertLookupValue(tableName, columns, "'Alemania', 'es', 100, 1, 0, 'DE', 'DEU', 49");
				InsertLookupValue(tableName, columns, "'Argelia', 'es', 103, 0, 0, 'DZ', 'DZA', 213");
				InsertLookupValue(tableName, columns, "'Samoa Americana', 'es', 104, 0, 0, 'AS', 'ASM', 1684");
				InsertLookupValue(tableName, columns, "'Andorra', 'es', 105, 0, 0, 'AD', 'AND', 376");
				InsertLookupValue(tableName, columns, "'Angola', 'es', 106, 0, 0, 'AO', 'AGO', 244");
				InsertLookupValue(tableName, columns, "'Anguilla', 'es', 107, 0, 0, 'AI', 'AIA', 1264");
				InsertLookupValue(tableName, columns, "'Antártica', 'es', 108, 0, 0, 'AQ', 'ATA', 6721");
				InsertLookupValue(tableName, columns, "'Antigua y Barbuda', 'es', 109, 0, 0, 'AG', 'ATG', 1268");
				InsertLookupValue(tableName, columns, "'Argentina', 'es', 110, 0, 0, 'AR', 'ARG', 54");
				InsertLookupValue(tableName, columns, "'Armenia', 'es', 111, 0, 0, 'AM', 'ARM', 374");
				InsertLookupValue(tableName, columns, "'Aruba', 'es', 112, 0, 0, 'AW', 'ABW', 297");
				InsertLookupValue(tableName, columns, "'Australia', 'es', 113, 0, 0, 'AU', 'AUS', 61");
				InsertLookupValue(tableName, columns, "'Austria', 'es', 114, 1, 1, 'AT', 'AUT', 43");
				InsertLookupValue(tableName, columns, "'Azerbaiyán', 'es', 115, 0, 0, 'AZ', 'AZE', 994");
				InsertLookupValue(tableName, columns, "'Bahamas', 'es', 116, 0, 0, 'BS', 'BHS', 1242");
				InsertLookupValue(tableName, columns, "'Baréin', 'es', 117, 0, 0, 'BH', 'BHR', 973");
				InsertLookupValue(tableName, columns, "'Bangladés', 'es', 118, 0, 0, 'BD', 'BGD', 880");
				InsertLookupValue(tableName, columns, "'Barbados', 'es', 119, 0, 0, 'BB', 'BRB', 1246");
				InsertLookupValue(tableName, columns, "'Belarús', 'es', 120, 0, 0, 'BY', 'BLR', 375");
				InsertLookupValue(tableName, columns, "'Bélgica', 'es', 121, 0, 0, 'BE', 'BEL', 32");
				InsertLookupValue(tableName, columns, "'Belice', 'es', 122, 0, 0, 'BZ', 'BLZ', 501");
				InsertLookupValue(tableName, columns, "'Benín', 'es', 123, 0, 0, 'BJ', 'BEN', 229");
				InsertLookupValue(tableName, columns, "'Bermudas', 'es', 124, 0, 0, 'BM', 'BMU', 1441");
				InsertLookupValue(tableName, columns, "'Bután', 'es', 125, 0, 0, 'BT', 'BTN', 975");
				InsertLookupValue(tableName, columns, "'Bolivia', 'es', 126, 0, 0, 'BO', 'BOL', 591");
				InsertLookupValue(tableName, columns, "'Bosnia y Herzegovina', 'es', 127, 0, 0, 'BA', 'BIH', 387");
				InsertLookupValue(tableName, columns, "'Botsuana', 'es', 128, 0, 0, 'BW', 'BWA', 267");
				InsertLookupValue(tableName, columns, "'Isla Bouvet', 'es', 129, 0, 0, 'BV', 'BVT', ''");
				InsertLookupValue(tableName, columns, "'Brasil', 'es', 130, 0, 0, 'BR', 'BRA', 55");
				InsertLookupValue(tableName, columns, "'Territorio Británico del Océano Índico', 'es', 131, 0, 0, 'IO', 'IOT', 246");
				InsertLookupValue(tableName, columns, "'Brunei Darussalam', 'es', 132, 0, 0, 'BN', 'BRN', 673");
				InsertLookupValue(tableName, columns, "'Bulgaria', 'es', 133, 0, 0, 'BG', 'BGR', 359");
				InsertLookupValue(tableName, columns, "'Burkina Faso', 'es', 134, 0, 0, 'BF', 'BFA', 226");
				InsertLookupValue(tableName, columns, "'Burundi', 'es', 135, 0, 0, 'BI', 'BDI', 226");
				InsertLookupValue(tableName, columns, "'Camboya', 'es', 136, 0, 0, 'KH', 'KHM', 855");
				InsertLookupValue(tableName, columns, "'Camerún', 'es', 137, 0, 0, 'CM', 'CMR', 237");
				InsertLookupValue(tableName, columns, "'Canadá', 'es', 138, 0, 0, 'CA', 'CAN', 1");
				InsertLookupValue(tableName, columns, "'Cabo Verde', 'es', 139, 0, 0, 'CV', 'CPV', 238");
				InsertLookupValue(tableName, columns, "'Islas Caimán', 'es', 140, 0, 0, 'KY', 'CYM', 1345");
				InsertLookupValue(tableName, columns, "'República Centroafricana', 'es', 141, 0, 0, 'CF', 'CAF', 236");
				InsertLookupValue(tableName, columns, "'Chad', 'es', 142, 0, 0, 'TD', 'TCD', 235");
				InsertLookupValue(tableName, columns, "'Chile', 'es', 143, 0, 0, 'CL', 'CHL', 56");
				InsertLookupValue(tableName, columns, "'China', 'es', 144, 0, 0, 'CN', 'CHN', 86");
				InsertLookupValue(tableName, columns, "'Isla Christmas', 'es', 145, 0, 0, 'CX', 'CXR', ''");
				InsertLookupValue(tableName, columns, "'Islas Cocos', 'es', 146, 0, 0, 'CC', 'CCK', ''");
				InsertLookupValue(tableName, columns, "'Colombia', 'es', 147, 0, 0, 'CO', 'COL', 57");
				InsertLookupValue(tableName, columns, "'Comoras', 'es', 148, 0, 0, 'KM', 'COM', 269");
				InsertLookupValue(tableName, columns, "'Congo', 'es', 149, 0, 0, 'CG', 'COG', 242");
				InsertLookupValue(tableName, columns, "'República Democrática del Congo', 'es', 150, 0, 0, 'CD', 'COD', 243");
				InsertLookupValue(tableName, columns, "'Islas Cook', 'es', 151, 0, 0, 'CK', 'COK', 682");
				InsertLookupValue(tableName, columns, "'Costa Rica', 'es', 152, 0, 0, 'CR', 'CRI', 506");
				InsertLookupValue(tableName, columns, "'Costa de Marfil', 'es', 153, 0, 0, 'CI', 'CIV', 225");
				InsertLookupValue(tableName, columns, "'Croacia', 'es', 154, 0, 0, 'HR', 'HRV', 385");
				InsertLookupValue(tableName, columns, "'Cuba', 'es', 155, 0, 0, 'CU', 'CUB', 53");
				InsertLookupValue(tableName, columns, "'Chipre', 'es', 156, 0, 0, 'CY', 'CYP', 357");
				InsertLookupValue(tableName, columns, "'República Checa', 'es', 157, 0, 0, 'CZ', 'CZE', 420");
				InsertLookupValue(tableName, columns, "'Dinamarca', 'es', 158, 0, 0, 'DK', 'DNK', 45");
				InsertLookupValue(tableName, columns, "'Yibuti', 'es', 159, 0, 0, 'DJ', 'DJI', 253");
				InsertLookupValue(tableName, columns, "'Dominica', 'es', 160, 0, 0, 'DM', 'DMA', 1767");
				InsertLookupValue(tableName, columns, "'República Dominicana', 'es', 161, 0, 0, 'DO', 'DOM', 1809");
				InsertLookupValue(tableName, columns, "'Ecuador', 'es', 162, 0, 0, 'EC', 'ECU', 593");
				InsertLookupValue(tableName, columns, "'Egipto', 'es', 163, 0, 0, 'EG', 'EGY', 20");
				InsertLookupValue(tableName, columns, "'El Salvador', 'es', 164, 0, 0, 'SV', 'SLV', 503");
				InsertLookupValue(tableName, columns, "'Guinea Ecuatorial', 'es', 165, 0, 0, 'GQ', 'GNQ', 240");
				InsertLookupValue(tableName, columns, "'Eritrea', 'es', 166, 0, 0, 'ER', 'ERI', 291");
				InsertLookupValue(tableName, columns, "'Estonia', 'es', 167, 0, 0, 'EE', 'EST', 372");
				InsertLookupValue(tableName, columns, "'Etiopía', 'es', 168, 0, 0, 'ET', 'ETH', 251");
				InsertLookupValue(tableName, columns, "'Islas Malvinas', 'es', 169, 0, 0, 'FK', 'FLK', 500");
				InsertLookupValue(tableName, columns, "'Islas Feroe', 'es', 170, 0, 0, 'FO', 'FRO', 298");
				InsertLookupValue(tableName, columns, "'Fiyi', 'es', 171, 0, 0, 'FJ', 'FJI', 679");
				InsertLookupValue(tableName, columns, "'Finlandia', 'es', 172, 0, 0, 'FI', 'FIN', 358");
				InsertLookupValue(tableName, columns, "'Francia', 'es', 173, 1, 4, 'FR', 'FRA', 33");
				InsertLookupValue(tableName, columns, "'Guyana Francesa', 'es', 174, 0, 0, 'GF', 'GUF', 594");
				InsertLookupValue(tableName, columns, "'Polinesia Francesa', 'es', 175, 0, 0, 'PF', 'PYF', 689");
				InsertLookupValue(tableName, columns, "'Tierras Australes y Antárticas Francesas', 'es', 176, 0, 0, 'TF', 'ATF', ''");
				InsertLookupValue(tableName, columns, "'Gabón', 'es', 177, 0, 0, 'GA', 'GAB', 241");
				InsertLookupValue(tableName, columns, "'Gambia', 'es', 178, 0, 0, 'GM', 'GMB', 220");
				InsertLookupValue(tableName, columns, "'Georgia', 'es', 179, 0, 0, 'GE', 'GEO', 995");
				InsertLookupValue(tableName, columns, "'Ghana', 'es', 180, 0, 0, 'GH', 'GHA', 233");
				InsertLookupValue(tableName, columns, "'Gibraltar', 'es', 181, 0, 0, 'GI', 'GIB', 350");
				InsertLookupValue(tableName, columns, "'Grecia', 'es', 182, 0, 0, 'GR', 'GRC', 30");
				InsertLookupValue(tableName, columns, "'Groenlandia', 'es', 183, 0, 0, 'GL', 'GRL', 299");
				InsertLookupValue(tableName, columns, "'Granada', 'es', 184, 0, 0, 'GD', 'GRD', 1473");
				InsertLookupValue(tableName, columns, "'Guadalupe', 'es', 185, 0, 0, 'GP', 'GLP', 590");
				InsertLookupValue(tableName, columns, "'Guam', 'es', 186, 0, 0, 'GU', 'GUM', 1671");
				InsertLookupValue(tableName, columns, "'Guatemala', 'es', 187, 0, 0, 'GT', 'GTM', 502");
				InsertLookupValue(tableName, columns, "'Guinea', 'es', 188, 0, 0, 'GN', 'GIN', 224");
				InsertLookupValue(tableName, columns, "'Guinea-Bisáu', 'es', 189, 0, 0, 'GW', 'GNB', 245");
				InsertLookupValue(tableName, columns, "'Guyana', 'es', 190, 0, 0, 'GY', 'GUY', 592");
				InsertLookupValue(tableName, columns, "'Haití', 'es', 191, 0, 0, 'HT', 'HTI', 509");
				InsertLookupValue(tableName, columns, "'Islas Heard y McDonald', 'es', 192, 0, 0, 'HM', 'HMD', ''");
				InsertLookupValue(tableName, columns, "'Estado de la Ciudad del Vaticano', 'es', 193, 0, 0, 'VA', 'VAT', 39");
				InsertLookupValue(tableName, columns, "'Honduras', 'es', 194, 0, 0, 'HN', 'HND', 504");
				InsertLookupValue(tableName, columns, "'Hong Kong', 'es', 195, 0, 0, 'HK', 'HKG', 852");
				InsertLookupValue(tableName, columns, "'Hungría', 'es', 196, 0, 0, 'HU', 'HUN', 36");
				InsertLookupValue(tableName, columns, "'Islandia', 'es', 197, 0, 0, 'IS', 'ISL', 354");
				InsertLookupValue(tableName, columns, "'India', 'es', 198, 0, 0, 'IN', 'IND', 91");
				InsertLookupValue(tableName, columns, "'Indonesia', 'es', 199, 0, 0, 'ID', 'IDN', 62");
				InsertLookupValue(tableName, columns, "'República Islámica de Irán', 'es', 200, 0, 0, 'IR', 'IRN', ''");
				InsertLookupValue(tableName, columns, "'Irak', 'es', 201, 0, 0, 'IQ', 'IRQ', 964");
				InsertLookupValue(tableName, columns, "'Irlanda', 'es', 202, 0, 0, 'IE', 'IRL', 353");
				InsertLookupValue(tableName, columns, "'Israel', 'es', 203, 0, 0, 'IL', 'ISR', 972");
				InsertLookupValue(tableName, columns, "'Italia', 'es', 204, 1, 5, 'IT', 'ITA', 39");
				InsertLookupValue(tableName, columns, "'Jamaica', 'es', 205, 0, 0, 'JM', 'JAM', 1876");
				InsertLookupValue(tableName, columns, "'Japón', 'es', 206, 0, 0, 'JP', 'JPN', 81");
				InsertLookupValue(tableName, columns, "'Jordania', 'es', 207, 0, 0, 'JO', 'JOR', 962");
				InsertLookupValue(tableName, columns, "'Kazajistán', 'es', 208, 0, 0, 'KZ', 'KAZ', 7");
				InsertLookupValue(tableName, columns, "'Kenia', 'es', 209, 0, 0, 'KE', 'KEN', 254");
				InsertLookupValue(tableName, columns, "'Kiribati', 'es', 210, 0, 0, 'KI', 'KIR', 686");
				InsertLookupValue(tableName, columns, "'República Popular Democrática de Corea', 'es', 211, 0, 0, 'KP', 'PRK', ''");
				InsertLookupValue(tableName, columns, "'República de Corea', 'es', 212, 0, 0, 'KR', 'KOR', 850");
				InsertLookupValue(tableName, columns, "'Kuwait', 'es', 213, 0, 0, 'KW', 'KWT', 965");
				InsertLookupValue(tableName, columns, "'Kirguistán', 'es', 214, 0, 0, 'KG', 'KGZ', 996");
				InsertLookupValue(tableName, columns, "'Laos', 'es', 215, 0, 0, 'LA', 'LAO', ''");
				InsertLookupValue(tableName, columns, "'Letonia', 'es', 216, 0, 0, 'LV', 'LVA', 371");
				InsertLookupValue(tableName, columns, "'Líbano', 'es', 217, 0, 0, 'LB', 'LBN', 961");
				InsertLookupValue(tableName, columns, "'Lesoto', 'es', 218, 0, 0, 'LS', 'LSO', 266");
				InsertLookupValue(tableName, columns, "'Liberia', 'es', 219, 0, 0, 'LR', 'LBR', 231");
				InsertLookupValue(tableName, columns, "'Jamahiriya Arabe Libia', 'es', 220, 0, 0, 'LY', 'LBY', 218");
				InsertLookupValue(tableName, columns, "'Liechtenstein', 'es', 221, 0, 0, 'LI', 'LIE', 423");
				InsertLookupValue(tableName, columns, "'Lituania', 'es', 222, 0, 0, 'LT', 'LTU', 370");
				InsertLookupValue(tableName, columns, "'Luxemburgo', 'es', 223, 0, 0, 'LU', 'LUX', 352");
				InsertLookupValue(tableName, columns, "'Macao', 'es', 224, 0, 0, 'MO', 'MAC', 853");
				InsertLookupValue(tableName, columns, "'Macedonia del Norte', 'es', 225, 0, 0, 'MK', 'MKD', ''");
				InsertLookupValue(tableName, columns, "'Madagascar', 'es', 226, 0, 0, 'MG', 'MDG', 261");
				InsertLookupValue(tableName, columns, "'Malaui', 'es', 227, 0, 0, 'MW', 'MWI', 265");
				InsertLookupValue(tableName, columns, "'Malasia', 'es', 228, 0, 0, 'MY', 'MYS', 60");
				InsertLookupValue(tableName, columns, "'Maldivas', 'es', 229, 0, 0, 'MV', 'MDV', 960");
				InsertLookupValue(tableName, columns, "'Mali', 'es', 230, 0, 0, 'ML', 'MLI', 223");
				InsertLookupValue(tableName, columns, "'Malta', 'es', 231, 0, 0, 'MT', 'MLT', 356");
				InsertLookupValue(tableName, columns, "'Islas Marshall', 'es', 232, 0, 0, 'MH', 'MHL', 692");
				InsertLookupValue(tableName, columns, "'Martinica', 'es', 233, 0, 0, 'MQ', 'MTQ', 596");
				InsertLookupValue(tableName, columns, "'Mauritania', 'es', 234, 0, 0, 'MR', 'MRT', 222");
				InsertLookupValue(tableName, columns, "'Mauricio', 'es', 235, 0, 0, 'MU', 'MUS', 230");
				InsertLookupValue(tableName, columns, "'Mayotte', 'es', 236, 0, 0, 'YT', 'MYT', 262");
				InsertLookupValue(tableName, columns, "'México', 'es', 237, 0, 0, 'MX', 'MEX', 52");
				InsertLookupValue(tableName, columns, "'Estados Federados de Micronesia', 'es', 238, 0, 0, 'FM', 'FSM', ''");
				InsertLookupValue(tableName, columns, "'República de Moldavia', 'es', 239, 0, 0, 'MD', 'MDA', ''");
				InsertLookupValue(tableName, columns, "'Mónaco', 'es', 240, 0, 0, 'MC', 'MCO', 377");
				InsertLookupValue(tableName, columns, "'Mongolia', 'es', 241, 0, 0, 'MN', 'MNG', 976");
				InsertLookupValue(tableName, columns, "'Montserrat', 'es', 242, 0, 0, 'MS', 'MSR', 1664");
				InsertLookupValue(tableName, columns, "'Marruecos', 'es', 243, 0, 0, 'MA', 'MAR', 212");
				InsertLookupValue(tableName, columns, "'Mozambique', 'es', 244, 0, 0, 'MZ', 'MOZ', 258");
				InsertLookupValue(tableName, columns, "'Myanmar', 'es', 245, 0, 0, 'MM', 'MMR', 95");
				InsertLookupValue(tableName, columns, "'Namibia', 'es', 246, 0, 0, 'NA', 'NAM', 264");
				InsertLookupValue(tableName, columns, "'Nauru', 'es', 247, 0, 0, 'NR', 'NRU', 674");
				InsertLookupValue(tableName, columns, "'Nepal', 'es', 248, 0, 0, 'NP', 'NPL', 977");
				InsertLookupValue(tableName, columns, "'Países Bajos', 'es', 249, 0, 0, 'NL', 'NLD', 31");
				InsertLookupValue(tableName, columns, "'Antillas Neerlandesas', 'es', 250, 0, 0, 'AN', 'ANT', 599");
				InsertLookupValue(tableName, columns, "'Nueva Caledonia', 'es', 251, 0, 0, 'NC', 'NCL', 687");
				InsertLookupValue(tableName, columns, "'Nueva Zelanda', 'es', 252, 0, 0, 'NZ', 'NZL', 64");
				InsertLookupValue(tableName, columns, "'Nicaragua', 'es', 253, 0, 0, 'NI', 'NIC', 505");
				InsertLookupValue(tableName, columns, "'Niger', 'es', 254, 0, 0, 'NE', 'NER', 227");
				InsertLookupValue(tableName, columns, "'Nigeria', 'es', 255, 0, 0, 'NG', 'NGA', 234");
				InsertLookupValue(tableName, columns, "'Niue', 'es', 256, 0, 0, 'NU', 'NIU', 683");
				InsertLookupValue(tableName, columns, "'Isla Norfolk', 'es', 257, 0, 0, 'NF', 'NFK', 6723");
				InsertLookupValue(tableName, columns, "'Islas Marianas del Norte', 'es', 258, 0, 0, 'MP', 'MNP', 1670");
				InsertLookupValue(tableName, columns, "'Noruega', 'es', 259, 0, 0, 'NO', 'NOR', 47");
				InsertLookupValue(tableName, columns, "'Omán', 'es', 260, 0, 0, 'OM', 'OMN', 968");
				InsertLookupValue(tableName, columns, "'Pakistán', 'es', 261, 0, 0, 'PK', 'PAK', 92");
				InsertLookupValue(tableName, columns, "'Palau', 'es', 262, 0, 0, 'PW', 'PLW', 680");
				InsertLookupValue(tableName, columns, "'Territorios Palestinos', 'es', 263, 0, 0, 'PS', 'PSE', ''");
				InsertLookupValue(tableName, columns, "'Panamá', 'es', 264, 0, 0, 'PA', 'PAN', 507");
				InsertLookupValue(tableName, columns, "'Papúa Nueva Guinea', 'es', 265, 0, 0, 'PG', 'PNG', 675");
				InsertLookupValue(tableName, columns, "'Paraguay', 'es', 266, 0, 0, 'PY', 'PRY', 595");
				InsertLookupValue(tableName, columns, "'Peru', 'es', 267, 0, 0, 'PE', 'PER', 51");
				InsertLookupValue(tableName, columns, "'Filipinas', 'es', 268, 0, 0, 'PH', 'PHL', 63");
				InsertLookupValue(tableName, columns, "'Pitcairn', 'es', 269, 0, 0, 'PN', 'PCN', ''");
				InsertLookupValue(tableName, columns, "'Polonia', 'es', 270, 0, 0, 'PL', 'POL', 48");
				InsertLookupValue(tableName, columns, "'Portugal', 'es', 271, 0, 0, 'PT', 'PRT', 351");
				InsertLookupValue(tableName, columns, "'Puerto Rico', 'es', 272, 0, 0, 'PR', 'PRI', 1787");
				InsertLookupValue(tableName, columns, "'Qatar', 'es', 273, 0, 0, 'QA', 'QAT', 974");
				InsertLookupValue(tableName, columns, "'Reunion', 'es', 274, 0, 0, 'RE', 'REU', 262");
				InsertLookupValue(tableName, columns, "'Rumanía', 'es', 275, 0, 0, 'RO', 'ROU', 40");
				InsertLookupValue(tableName, columns, "'Federación de Rusia', 'es', 276, 0, 0, 'RU', 'RUS', 7");
				InsertLookupValue(tableName, columns, "'Ruanda', 'es', 277, 0, 0, 'RW', 'RWA', 250");
				InsertLookupValue(tableName, columns, "'Santa Elena', 'es', 278, 0, 0, 'SH', 'SHN', 290");
				InsertLookupValue(tableName, columns, "'San Cristóbal y Nieves', 'es', 279, 0, 0, 'KN', 'KNA', 1869");
				InsertLookupValue(tableName, columns, "'Santa Lucia', 'es', 280, 0, 0, 'LC', 'LCA', 1758");
				InsertLookupValue(tableName, columns, "'San Pedro y Miquelón', 'es', 281, 0, 0, 'PM', 'SPM', 508");
				InsertLookupValue(tableName, columns, "'San Vicente y las Granadinas', 'es', 282, 0, 0, 'VC', 'VCT', 1784");
				InsertLookupValue(tableName, columns, "'Samoa', 'es', 283, 0, 0, 'WS', 'WSM', 685");
				InsertLookupValue(tableName, columns, "'San Marino', 'es', 284, 0, 0, 'SM', 'SMR', 378");
				InsertLookupValue(tableName, columns, "'Santo Tomé y Príncipe', 'es', 285, 0, 0, 'ST', 'STP', 239");
				InsertLookupValue(tableName, columns, "'Arabia Saudí', 'es', 286, 0, 0, 'SA', 'SAU', 966");
				InsertLookupValue(tableName, columns, "'Senegal', 'es', 287, 0, 0, 'SN', 'SEN', 221");
				InsertLookupValue(tableName, columns, "'Serbia', 'es', 288, 0, 0, 'YU', 'YUG', ''");
				InsertLookupValue(tableName, columns, "'Seychelles', 'es', 289, 0, 0, 'SC', 'SYC', 248");
				InsertLookupValue(tableName, columns, "'Sierra Leone', 'es', 290, 0, 0, 'SL', 'SLE', 232");
				InsertLookupValue(tableName, columns, "'Singapur', 'es', 291, 0, 0, 'SG', 'SGP', 65");
				InsertLookupValue(tableName, columns, "'Eslovaquia', 'es', 292, 0, 0, 'SK', 'SVK', 421");
				InsertLookupValue(tableName, columns, "'Eslovenia', 'es', 293, 0, 0, 'SI', 'SVN', 386");
				InsertLookupValue(tableName, columns, "'Islas Salomón', 'es', 294, 0, 0, 'SB', 'SLB', 677");
				InsertLookupValue(tableName, columns, "'Somalia', 'es', 295, 0, 0, 'SO', 'SOM', 252");
				InsertLookupValue(tableName, columns, "'Sudáfrica', 'es', 296, 0, 0, 'ZA', 'ZAF', 27");
				InsertLookupValue(tableName, columns, "'Islas Georgias del Sur y Sandwich del Sur', 'es', 297, 0, 0, 'GS', 'SGS', ''");
				InsertLookupValue(tableName, columns, "'España', 'es', 298, 0, 0, 'ES', 'ESP', 34");
				InsertLookupValue(tableName, columns, "'Sri Lanka', 'es', 299, 0, 0, 'LK', 'LKA', 94");
				InsertLookupValue(tableName, columns, "'Sudán', 'es', 300, 0, 0, 'SD', 'SDN', 249");
				InsertLookupValue(tableName, columns, "'Suriname', 'es', 301, 0, 0, 'SR', 'SUR', 597");
				InsertLookupValue(tableName, columns, "'Svalbard y Jan Mayen', 'es', 302, 0, 0, 'SJ', 'SJM', 47");
				InsertLookupValue(tableName, columns, "'Swaziland', 'es', 303, 0, 0, 'SZ', 'SWZ', 268");
				InsertLookupValue(tableName, columns, "'Suecia', 'es', 304, 0, 0, 'SE', 'SWE', 46");
				InsertLookupValue(tableName, columns, "'Suiza', 'es', 305, 1, 2, 'CH', 'CHE', 41");
				InsertLookupValue(tableName, columns, "'República Árabe Siria', 'es', 306, 0, 0, 'SY', 'SYR', 963");
				InsertLookupValue(tableName, columns, "'Taiwan', 'es', 307, 0, 0, 'TW', 'TWN', 886");
				InsertLookupValue(tableName, columns, "'Tayikistán', 'es', 308, 0, 0, 'TJ', 'TJK', 992");
				InsertLookupValue(tableName, columns, "'República Unida de Tanzania', 'es', 309, 0, 0, 'TZ', 'TZA', 255");
				InsertLookupValue(tableName, columns, "'Tailandia', 'es', 310, 0, 0, 'TH', 'THA', 66");
				InsertLookupValue(tableName, columns, "'Timor Oriental', 'es', 311, 0, 0, 'TL', 'TLS', 670");
				InsertLookupValue(tableName, columns, "'Togo', 'es', 312, 0, 0, 'TG', 'TGO', 228");
				InsertLookupValue(tableName, columns, "'Tokelau', 'es', 313, 0, 0, 'TK', 'TKL', 690");
				InsertLookupValue(tableName, columns, "'Tonga', 'es', 314, 0, 0, 'TO', 'TON', 676");
				InsertLookupValue(tableName, columns, "'Trinidad and Tobago', 'es', 315, 0, 0, 'TT', 'TTO', 1868");
				InsertLookupValue(tableName, columns, "'Túnez', 'es', 316, 0, 0, 'TN', 'TUN', 216");
				InsertLookupValue(tableName, columns, "'Turquía', 'es', 317, 0, 0, 'TR', 'TUR', 90");
				InsertLookupValue(tableName, columns, "'Turkmenistán', 'es', 318, 0, 0, 'TM', 'TKM', 993");
				InsertLookupValue(tableName, columns, "'Islas Turcas y Caicos', 'es', 319, 0, 0, 'TC', 'TCA', 1649");
				InsertLookupValue(tableName, columns, "'Tuvalu', 'es', 320, 0, 0, 'TV', 'TUV', 688");
				InsertLookupValue(tableName, columns, "'Uganda', 'es', 321, 0, 0, 'UG', 'UGA', 256");
				InsertLookupValue(tableName, columns, "'Ucrania', 'es', 322, 0, 0, 'UA', 'UKR', 380");
				InsertLookupValue(tableName, columns, "'Emiratos Árabes Unidos', 'es', 323, 0, 0, 'AE', 'ARE', 971");
				InsertLookupValue(tableName, columns, "'Reino Unido', 'es', 324, 1, 3, 'GB', 'GBR', 44");
				InsertLookupValue(tableName, columns, "'Estados Unidos', 'es', 325, 1, 6, 'US', 'USA', 1");
				InsertLookupValue(tableName, columns, "'Islas menores alejadas de los Estados Unidos', 'es', 326, 0, 0, 'UM', 'UMI', ''");
				InsertLookupValue(tableName, columns, "'Uruguay', 'es', 327, 0, 0, 'UY', 'URY', 598");
				InsertLookupValue(tableName, columns, "'Uzbekistán', 'es', 328, 0, 0, 'UZ', 'UZB', 998");
				InsertLookupValue(tableName, columns, "'Vanuatu', 'es', 329, 0, 0, 'VU', 'VUT', 678");
				InsertLookupValue(tableName, columns, "'Venezuela', 'es', 330, 0, 0, 'VE', 'VEN', 58");
				InsertLookupValue(tableName, columns, "'Vietnam', 'es', 331, 0, 0, 'VN', 'VNM', 84");
				InsertLookupValue(tableName, columns, "'Islas Vírgenes Británicas', 'es', 332, 0, 0, 'VG', 'VGB', 1284");
				InsertLookupValue(tableName, columns, "'Islas Vìrgenes de EE.UU', 'es', 333, 0, 0, 'VI', 'VIR', 1340");
				InsertLookupValue(tableName, columns, "'Wallis y Futuna', 'es', 334, 0, 0, 'WF', 'WLF', 681");
				InsertLookupValue(tableName, columns, "'Sáhara Occidental', 'es', 335, 0, 0, 'EH', 'ESH', 212");
				InsertLookupValue(tableName, columns, "'Yemen', 'es', 336, 0, 0, 'YE', 'YEM', 967");
				InsertLookupValue(tableName, columns, "'Zambia', 'es', 337, 0, 0, 'ZM', 'ZMB', 260");
				InsertLookupValue(tableName, columns, "'Zimbabue', 'es', 338, 0, 0, 'ZW', 'ZWE', 263");
			}
			tableName = "[LU].[Turnover]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'No se aplica', 'es', '100', 0, 0");
				InsertLookupValue(tableName, columns, "'Menos de 10M', 'es', '101', 0, 1");
				InsertLookupValue(tableName, columns, "'10M - 24M', 'es', '102', 0, 2");
				InsertLookupValue(tableName, columns, "'25M - 99M', 'es', '103', 0, 3");
				InsertLookupValue(tableName, columns, "'100M - 249M', 'es', '104', 0, 4");
				InsertLookupValue(tableName, columns, "'250M - 499M', 'es', '105', 0, 5");
				InsertLookupValue(tableName, columns, "'Más de 500M', 'es', '106', 0, 6");
			}
			tableName = "[LU].[UserStatus]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'Disponible', 'es', 'Available', 0, 0");
				InsertLookupValue(tableName, columns, "'No disponible', 'es', 'Away', 0, 0");
				InsertLookupValue(tableName, columns, "'No molestar', 'es', 'DnD', 0, 0");
			}
			tableName = "[SMS].[Skill]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value" };
				InsertLookupValue(tableName, columns, "'Electricos', 'es', 'Electrics'");
				InsertLookupValue(tableName, columns, "'Hidráulica', 'es', 'Hydraulics'");
				InsertLookupValue(tableName, columns, "'Red', 'es', 'Network'");
				InsertLookupValue(tableName, columns, "'Mecánico', 'es', 'Mechanics'");
				InsertLookupValue(tableName, columns, "'Software', 'es', 'Software'");
			}
		}
		private void InsertLookupValue(string tableName, string[] columns, string values, bool hasIsActiveColumn = true)
		{
			int keyColumnIndex = Array.IndexOf(columns, "Value");
			string keyValue = values.Split(',')[keyColumnIndex].Trim(new char[] { ' ', '\'' });
			if ((int)Database.ExecuteScalar($"SELECT COUNT(*) FROM {tableName} WHERE {(hasIsActiveColumn ? "[IsActive]" : 1)} = 1 AND [Value] = '{keyValue}'") > 0)
			{
				Database.ExecuteNonQuery($"INSERT INTO {tableName} ({string.Join(", ", columns)}) VALUES ({values})");
			}
		}
	}
}
