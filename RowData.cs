using System.Collections.Generic;
using System.Drawing;

namespace BeefThemeEditor
{
	public class RowData
	{
		public string Name;
		public string Comment;
		public string HexValue;
		public string NewHexValue;
		public Color Colour;
		public Color NewColour;
		public Bitmap CurrImg;
		public Bitmap NewImg;
		public int Ix;
		public bool Updated = false;
		public int X;
		public int Y;

		public void Set(string hexValue, Color color)
		{
			HexValue=hexValue;
			NewHexValue= null;
			Colour= color;
			NewColour = new System.Drawing.Color();
		}

		public enum ImageIdx
		{
			// Line 1 of .psd
			Bkg,
			Window,
			Dots,
			RadioOn,
			RadioOff,
			MainBtnUp,
			MainBtnDown,
			BtnUp,
			BtnOver,
			BtnDown,
			Separator,
			TabActive,
			TabActiveOver,
			TabInactive,
			TabInactiveOver,
			EditBox,
			Checkbox,
			CheckboxOver,
			CheckboxDown,
			Check,

			// Line 2 of .psd
			Close,
			CloseOver,
			DownArrow,
			GlowDot,
			ArrowRight,
			WhiteCircle,
			DropMenuButton,
			ListViewHeader,
			ListViewSortArrow,
			Outline,
			Scrollbar,
			ScrollbarThumbOver,
			ScrollbarThumb,
			ScrollbarArrow,
			ShortButton,
			ShortButtonDown,
			VertScrollbar,
			VertScrollbarThumbOver,
			VertScrollbarThumb,
			VertScrollbarArrow,

			// Line 3 of .psd
			VertShortButton,
			VertShortButtonDown,
			Grabber,
			DropShadow,
			Menu,
			MenuSepVert,
			MenuSepHorz,
			MenuSelect,
			TreeArrow,
			UIPointer,
			UIImage,
			UIComposition,
			UILabel,
			UIButton,
			UIEdit,
			UICombobox,
			UICheckbox,
			UIRadioButton,
			UIListView,
			UITabView,

			// Line 4 of .psd
			EditCorners,
			EditCircle,
			EditPathNode,
			EditPathNodeSelected,
			EditAnchor,
			UIBone,
			UIBoneJoint,
			VisibleIcon,
			LockIcon,
			LeftArrow,
			KeyframeMakeOff,
			RightArrow,
			LeftArrowDisabled,
			KeyframeMakeOn,
			RightArrowDisabled,
			TimelineSelector,
			TimelineBracket,
			KeyframeOff,
			KeyframeOn,
			LinkedIcon,

			// Line 5 of .psd
			CheckboxLarge,
			ComboBox,
			ComboEnd,
			ComboSelectedIcon,
			LinePointer,
			RedDot,
			Document,
			ReturnPointer,
			RefreshArrows,
			MoveDownArrow,
			IconObject,
			IconObjectDeleted,
			IconObjectAppend,
			IconObjectStack,
			IconValue,
			IconPointer,
			IconType,
			IconError,
			IconBookmark,
			ProjectFolder,

			// Line 6 of .psd
			Project,
			ArrowMoveDown,
			Workspace,
			MemoryArrowSingle,
			MemoryArrowDoubleTop,
			MemoryArrowDoubleBottom,
			MemoryArrowTripleTop,
			MemoryArrowTripleMiddle,
			MemoryArrowTripleBottom,
			MemoryArrowRainbow,
			Namespace,
			ResizeGrabber,
			AsmArrow,
			AsmArrowRev,
			AsmArrowShadow,
			MenuNonFocusSelect,
			StepFilter,
			WaitSegment,
			FindCaseSensitive,
			FindWholeWord,

			// Line 7 of .psd
			RedDotUnbound,
			MoreInfo,
			Interface,
			Property,
			Field,
			Method,
			Variable,
			Constant,
			Type_ValueType,
			Type_Class,
			LinePointer_Prev,
			LinePointer_Opt,
			RedDotEx,
			RedDotExUnbound,
			RedDotDisabled,
			RedDotExDisabled,
			RedDotRunToCursor,
			GotoButton,
			YesJmp,
			NoJmp,

			// Line 8 of .psd
			WhiteBox,
			UpDownArrows,
			EventInfo,
			WaitBar,
			HiliteOutline,
			HiliteOutlineThin,
			IconPayloadEnum,
			StepFilteredDefault,
			ThreadBreakpointMatch,
			ThreadBreakpointNoMatch,
			ThreadBreakpointUnbound,
			Search,
			CheckIndeterminate,
			CodeError,
			CodeWarning,
			ComboBoxFrameless,
			PanelHeader,
			ExtMethod,

			COUNT
		};
	}
}
