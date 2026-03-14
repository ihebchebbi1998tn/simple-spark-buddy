namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160512170011)]
	public class AddServiceOrderDispatchRejectReasons : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				IF (SELECT TOP 1 ServiceOrderStatus FROM [SMS].[ServiceOrderDispatchRejectReason] WHERE [Value] = 'FalseAlarm' AND [Language] = 'de') is null
				BEGIN
					UPDATE [SMS].[ServiceOrderDispatchRejectReason] SET ServiceOrderStatus = 'Completed', ModifyDate = GETUTCDATE() WHERE [Value] = 'FalseAlarm';
				END;
				IF (SELECT TOP 1 ServiceOrderStatus FROM [SMS].[ServiceOrderDispatchRejectReason] WHERE [Value] = 'ConflictingDates' AND [Language] = 'de') is null
				BEGIN
					UPDATE [SMS].[ServiceOrderDispatchRejectReason] SET ServiceOrderStatus = 'ReadyForScheduling', ModifyDate = GETUTCDATE() WHERE [Value] = 'ConflictingDates';
				END;
				IF (SELECT TOP 1 ServiceOrderStatus FROM [SMS].[ServiceOrderDispatchRejectReason] WHERE [Value] = 'CustomerNotAccessible' AND [Language] = 'de') is null
				BEGIN
					UPDATE [SMS].[ServiceOrderDispatchRejectReason] SET ServiceOrderStatus = 'ReadyForScheduling', ModifyDate = GETUTCDATE() WHERE [Value] = 'CustomerNotAccessible';
				END;
				IF (SELECT TOP 1 ServiceOrderStatus FROM [SMS].[ServiceOrderDispatchRejectReason] WHERE [Value] = 'InstallationNotAccessible' AND [Language] = 'de') is null
				BEGIN
					UPDATE [SMS].[ServiceOrderDispatchRejectReason] SET ServiceOrderStatus = 'ReadyForScheduling', ModifyDate = GETUTCDATE() WHERE [Value] = 'InstallationNotAccessible';
				END;
		");
		}

		public override void Down()
		{
		}
	}
}
