var audio = document.getElementById('audioFile');
var progressBar = document.getElementById('audioProgressBar');
var progress = document.getElementById('audioProgress');
var playPauseButton = document.getElementById('playPauseButton');
var audioPlayer = document.querySelector('.audio-player');
var currentTimeElem = document.getElementById('currentTime');
var totalTimeElem = document.getElementById('totalTime');

// Toggle play/pause functionality and button appearance
function togglePlayPause() {
    if (audio.paused || audio.ended) {
        audio.play();
        audioPlayer.classList.add('audio-playing');
    } else {
        audio.pause();
        audioPlayer.classList.remove('audio-playing');
    }
}

// Update the progress bar
audio.ontimeupdate = function() {
    var percentage = Math.floor((100 / audio.duration) * audio.currentTime);
    progress.style.width = percentage + '%';
    currentTimeElem.textContent = formatTime(audio.currentTime);
};

// Click on progress bar to change the time
progressBar.addEventListener('click', function(e) {
    var offset = this.getBoundingClientRect();
    var x = e.pageX - offset.left;
    audio.currentTime = (x / progressBar.offsetWidth) * audio.duration;
});

// Adjust progress element to grow from left to right
function skipTime(seconds) {
    audio.currentTime += seconds;
}

// Format time in minutes:seconds
function formatTime(seconds) {
    var minutes = Math.floor(seconds / 60);
    var seconds = Math.floor(seconds % 60);
    return minutes + ':' + (seconds < 10 ? '0' + seconds : seconds);
}

// Set the total time display
audio.onloadedmetadata = function() {
    totalTimeElem.textContent = formatTime(audio.duration);
};
function changeSpeed(speed) {
    audio.playbackRate = parseFloat(speed);
}


// ---- Điều chỉnh tốc độ phát ----
function changeSpeed(speed) {
    audio.playbackRate = speed;

    // Đổi trạng thái active cho nút được chọn
    document.querySelectorAll('.speed-controls button').forEach(btn => {
        btn.classList.remove('active');
    });
    event.target.classList.add('active');
}

// ---- Điều chỉnh âm lượng ----
function changeVolume(amount) {
    audio.volume = Math.min(1, Math.max(0, audio.volume + amount));
    document.getElementById('volumeSlider').value = audio.volume;
}

// ---- Cập nhật volume khi kéo thanh ----
document.getElementById('volumeSlider').addEventListener('input', function() {
    audio.volume = this.value;
});

// ---- (Có thể giữ lại nếu dùng jQuery cho phần khác) ----
$('.choose-voice').click(function (event) {
    event.preventDefault();
    $(this).toggleClass('dropdown_toggle');
});
