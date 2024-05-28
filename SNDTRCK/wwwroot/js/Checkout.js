
//Längden av animationen
const animationDuration = 2000;
const frameDuration = 1000 / 60;
const totalFrames = Math.round(animationDuration / frameDuration);
const easeOutQuad = t => t * (2 - t);

// The animation function, which takes an Element
const animateCountUp = el => {
	let frame = 0;
	const countTo = parseInt(el.innerHTML, 10);
	// Start the animation running 60 times per second
	const counter = setInterval(() => {
		frame++;
		// Calculate our progress as a value between 0 and 1
		// Pass that value to our easing function to get our
		// progress on a curve
		const progress = easeOutQuad(frame / totalFrames);
		// Use the progress value to calculate the current count
		const currentCount = Math.round(countTo * progress);

		// If the current count has changed, update the element
		if (parseInt(el.innerHTML, 10) !== currentCount) {
			el.innerHTML = currentCount;
		}

		// If we’ve reached our last frame, stop the animation
		if (frame === totalFrames) {
			clearInterval(counter);
		}
	}, frameDuration);
};

// Run the animation on all elements with a class of ‘countup’
const runAnimations = () => {
	const countupEls = document.querySelectorAll('.countUp');
	console.log("test")
	countupEls.forEach(animateCountUp);
};

//Startup
runAnimations();