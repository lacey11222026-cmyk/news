
// ===== Countdown (hiển thị 2 chữ số, KHÔNG tách digit) =====
(function () {
  // ==== 1) Mốc thời gian cần đếm tới ====
  // Ví dụ: 31/12/2025 23:59:59 (giờ theo trình duyệt)
  const TARGET = new Date("2026-03-30T23:59:59");

  // ==== 2) Hằng số thời gian ====
  const second = 1000;
  const minute = 60 * second;
  const hour   = 60 * minute;
  const day    = 24 * hour;

  // ==== 3) Set giá trị với format 2 chữ số ====
  function setValue(id, value) {
    const el = document.getElementById(id);
    if (!el) return;
    el.textContent = String(value).padStart(2, "0");
  }

  // ==== 4) Hàm cập nhật countdown ====
  function tick() {
    const now = Date.now();
    let distance = TARGET.getTime() - now;

    // Hết giờ
    if (distance <= 0) {
      ["days", "hours", "minutes", "seconds"].forEach(id => setValue(id, 0));
      clearInterval(timer);
      return;
    }

    const d = Math.floor(distance / day);
    distance %= day;

    const h = Math.floor(distance / hour);
    distance %= hour;

    const m = Math.floor(distance / minute);
    distance %= minute;

    const s = Math.floor(distance / second);

    setValue("days", d);
    setValue("hours", h);
    setValue("minutes", m);
    setValue("seconds", s);
  }

  // Chạy lần đầu
  tick();

  // Cập nhật mỗi giây
  const timer = setInterval(tick, 1000);
})();
