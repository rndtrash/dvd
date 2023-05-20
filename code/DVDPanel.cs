using Sandbox.UI;

namespace DVD;

public partial class DVDPanel : Panel
{
	public override void Tick()
	{
		Style.Left = Length.Percent( DVDEntity.Instance.xConverted * 100 );
		Style.Top = Length.Percent( DVDEntity.Instance.yConverted * 100 );

		Style.BackgroundTint = DVDEntity.HexColors[DVDEntity.Instance.currentColor];
	}
}
