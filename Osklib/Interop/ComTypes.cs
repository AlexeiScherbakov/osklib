using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Osklib.Interop
{
	internal static class ComTypes
	{
		internal static readonly Guid ImmersiveShellBrokerGuid;
		internal static readonly Type ImmersiveShellBrokerType;

		internal static readonly Guid TipInvocationGuid;
		internal static readonly Type TipInvocationType;

		static ComTypes()
		{
			TipInvocationGuid = Guid.Parse("4ce576fa-83dc-4F88-951c-9d0782b4e376");
			TipInvocationType = Type.GetTypeFromCLSID(TipInvocationGuid);

			ImmersiveShellBrokerGuid = new Guid("228826af-02e1-4226-a9e0-99a855e455a6");
			ImmersiveShellBrokerType = Type.GetTypeFromCLSID(ImmersiveShellBrokerGuid);
		}
	}
}
