using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BeefThemeEditor
{
	public class RowData
	{
		public enum Type : byte
		{
			toml,png
		}
		public string fileName;
		public List<string> hexValues = new List<string>();
		public List<string> newHexValues = new List<string>();
		public List<Color> colours = new List<Color>();
		public List<Color> newColours = new List<Color>();
		public Bitmap currImg;
		public Bitmap newImg;
		public int ix;
		public Type type;
		public int scale=1;
		public bool updated = false;

		public RowData(string file, Type type)
		{
			fileName = file;
			this.type = type;
		}

		public void Add(string hexValue, Color color)
		{
			hexValues.Add(hexValue);
			newHexValues.Add(null);
			colours.Add(color);
			newColours.Add(new System.Drawing.Color());
		}

		public int GetIx(string hexValue)
		{
			for (ix = 0; ix < hexValues.Count; ix++)
			{
				if (hexValues[ix] == hexValue)
				{
					break;
				}
			}
			return ix;
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
