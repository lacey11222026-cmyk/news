// $Id: calendar-en.js 6573 2007-03-09 08:36:16Z slip $
// ** I18N

// Calendar EN language
// Author: Mihai Bazon, <mihai_bazon@yahoo.com>
// Encoding: any
// Distributed under the same terms as the calendar itself.

// For translators: please use UTF-8 if possible.  We strongly believe that
// Unicode is the answer to a real internationalized world.  Also please
// include your contact information in the header, as can be seen above.

// full day names
Zapatec.Calendar._DN = new Array
("Sunday",
 "Monday",
 "Tuesday",
 "Wednesday",
 "Thursday",
 "Friday",
 "Saturday",
 "Sunday");

// Please note that the following array of short day names (and the same goes
// for short month names, _SMN) isn't absolutely necessary.  We give it here
// for exemplification on how one can customize the short day names, but if
// they are simply the first N letters of the full name you can simply say:
//
//   Zapatec.Calendar._SDN_len = N; // short day name length
//   Zapatec.Calendar._SMN_len = N; // short month name length
//
// If N = 3 then this is not needed either since we assume a value of 3 if not
// present, to be compatible with translation files that were written before
// this feature.

// short day names
Zapatec.Calendar._SDN = new Array
("Ch&#7911; nh&#7853;t",
 "Th&#7913; 2",
 "Th&#7913; 3",
 "Th&#7913; 4",
 "Th&#7913; 5",
 "Th&#7913; 6",
 "Th&#7913; 7",
 "Ch&#7911; nh&#7853;t");

// First day of the week. "0" means display Sunday first, "1" means display
// Monday first, etc.
Zapatec.Calendar._FD = 0;

// full month names
Zapatec.Calendar._MN = new Array
("Th&#225;ng 1",
 "Th&#225;ng 2",
 "Th&#225;ng 3",
 "Th&#225;ng 4",
 "Th&#225;ng 5",
 "Th&#225;ng 6",
 "Th&#225;ng 7",
 "Th&#225;ngt 8",
 "Th&#225;ng 9",
 "Th&#225;ng 10",
 "Th&#225;ng 11",
 "Th&#225;ng 12");

// short month names
Zapatec.Calendar._SMN = new Array
("Th&#225;ng 1",
 "Th&#225;ng 2",
 "Th&#225;ng 3",
 "Th&#225;ng 4",
 "Th&#225;ng 5",
 "Th&#225;ng 6",
 "Th&#225;ng 7",
 "Th&#225;ngt 8",
 "Th&#225;ng 9",
 "Th&#225;ng 10",
 "Th&#225;ng 11",
 "Th&#225;ng 12");

// tooltips
Zapatec.Calendar._TT_en = Zapatec.Calendar._TT = {};
Zapatec.Calendar._TT["INFO"] = "Giới thiệu";

Zapatec.Calendar._TT["ABOUT"] =
"Bộ chọn ngày / thời gian DHTML\n" +
"(c) zapatec.com 2002-2007\n" + // don't translate this this ;-)
"Để có phiên bản mới nhất bạn hãy vào trang: http://www.zapatec.com/\n" +
"\n\n" +
"Chọn ngày:\n" +
"- Sử dụng nút \xab, \xbb để chọn năm\n" +
"- Sử dụng nút " + String.fromCharCode(0x2039) + ", " + String.fromCharCode(0x203a) + " để chọn tháng\n" +
"- Giữ chuột trên bất kỳ các nút ở trên để lựa chọn nhanh hơn.";
Zapatec.Calendar._TT["ABOUT_TIME"] = "\n\n" +
"Chọn thời gian:\n" +
"- Click on any of the time parts to increase it\n" +
"- or Shift-click to decrease it\n" +
"- or click and drag for faster selection.";

Zapatec.Calendar._TT["PREV_YEAR"] = "N&#259;m tr&#432;&#7899;c";
Zapatec.Calendar._TT["PREV_MONTH"] = "Th&#225;ng tr&#432;&#7899;c";
Zapatec.Calendar._TT["GO_TODAY"] = "Ngày hôm nay";
Zapatec.Calendar._TT["NEXT_MONTH"] = "Th&#225;ng sau";
Zapatec.Calendar._TT["NEXT_YEAR"] = "N&#259;m sau";
Zapatec.Calendar._TT["SEL_DATE"] = "Chọn ngày";
Zapatec.Calendar._TT["DRAG_TO_MOVE"] = " K&#233;o &#273;&#7875; di chuy&#7875;n";
Zapatec.Calendar._TT["PART_TODAY"] = " (hôm nay)";

// the following is to inform that "%s" is to be the first day of week
// %s will be replaced with the day name.
Zapatec.Calendar._TT["DAY_FIRST"] = "Display %s first";

// This may be locale-dependent.  It specifies the week-end days, as an array
// of comma-separated numbers.  The numbers are from 0 to 6: 0 means Sunday, 1
// means Monday, etc.
Zapatec.Calendar._TT["WEEKEND"] = "0,6";

Zapatec.Calendar._TT["CLOSE"] = "&#272;&#243;ng";
Zapatec.Calendar._TT["TODAY"] = "Hôm nay";
Zapatec.Calendar._TT["TIME_PART"] = "(Shift-)Click or drag to change value";

// date formats
Zapatec.Calendar._TT["DEF_DATE_FORMAT"] = "%Y-%m-%d";
Zapatec.Calendar._TT["TT_DATE_FORMAT"] = "%a, ng&#224;y %e, %b ";

Zapatec.Calendar._TT["WK"] = "Tuần";
Zapatec.Calendar._TT["TIME"] = "Time:";

Zapatec.Calendar._TT["E_RANGE"] = "Outside the range";

Zapatec.Calendar._TT._AMPM = {am : "am", pm : "pm"};

/* Preserve data */
	if(Zapatec.Calendar._DN) Zapatec.Calendar._TT._DN = Zapatec.Calendar._DN;
	if(Zapatec.Calendar._SDN) Zapatec.Calendar._TT._SDN = Zapatec.Calendar._SDN;
	if(Zapatec.Calendar._SDN_len) Zapatec.Calendar._TT._SDN_len = Zapatec.Calendar._SDN_len;
	if(Zapatec.Calendar._MN) Zapatec.Calendar._TT._MN = Zapatec.Calendar._MN;
	if(Zapatec.Calendar._SMN) Zapatec.Calendar._TT._SMN = Zapatec.Calendar._SMN;
	if(Zapatec.Calendar._SMN_len) Zapatec.Calendar._TT._SMN_len = Zapatec.Calendar._SMN_len;
	Zapatec.Calendar._DN = Zapatec.Calendar._SDN = Zapatec.Calendar._SDN_len = Zapatec.Calendar._MN = Zapatec.Calendar._SMN = Zapatec.Calendar._SMN_len = null
