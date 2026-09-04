$(function () {
// ===== AUDIO PLAYER =====
	var audio = document.getElementById('audioFile');
	var progressBar = document.getElementById('progressBar');
	var progress = document.getElementById('progress');
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

	// Click on progress bar to change the time
	if (progressBar) {
		progressBar.addEventListener('click', function (e) {
			var offset = this.getBoundingClientRect();
			var x = e.pageX - offset.left;
			audio.currentTime = (x / progressBar.offsetWidth) * audio.duration;
		});
	}

	// Format time in minutes:seconds
	function formatTime(seconds) {
		var minutes = Math.floor(seconds / 60);
		var seconds = Math.floor(seconds % 60);
		return minutes + ':' + (seconds < 10 ? '0' + seconds : seconds);
	}

	// Update the current time display and progress bar
	if (audio) {
		audio.ontimeupdate = function () {
			var percentage = Math.floor((100 / audio.duration) * audio.currentTime);
			progress.style.width = percentage + '%';
			currentTimeElem.textContent = formatTime(audio.currentTime);
		};

		audio.onloadedmetadata = function () {
			totalTimeElem.textContent = formatTime(audio.duration);
		};
	}

	if (playPauseButton) {
		playPauseButton.addEventListener('click', togglePlayPause);
	}

	// Dropdown toggle
	$('.dropdown__inner').click(function (event) {
		event.preventDefault();
		$(this).toggleClass('dropdown_toggle');
	});

});