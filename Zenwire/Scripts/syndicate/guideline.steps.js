var githubGuide = new Guideline.Guide("welcome");

var tour = githubGuide.addPage("tour");

tour.addStep({
	type: "overlay",
	showAt: "#home",
    content:
		"<div class='gl-overlay'>"+
			"<h1>Welcome to Zenwire!</h1>"+
			"<p>We would love to give you a tour around.</p>"+
			"<button class='btn btn-syndicate squared start-tour'>Start tour</button>"+
			" "+
			"<button class='btn btn-eco squared gl-skip'>Skip tour</button>"+
		"</div>",
	continueWhen: "click .start-tour",
});

tour.addStep({
	type: "bubble",
	title: "Our Services",
	content: "Our services are where you will find out about all the amazing talents our professionals can do.",
	showAt: "#services",
	align: "center middle",
	showContinue: true
});

tour.addStep({
	type: "bubble",
	title: "Our Work",
	content: "We're quite proud of the workmanship our team shows at every call. Just like our mother, we love to hang our work.",
	showAt: "#work",
	align: "center middle",
	showContinue: true
});

tour.addStep({
	type: "bubble",
	title: "Get In Touch",
	content: "We thank you for taking the tour with us. If you need anything please contact us.",
	showAt: "#contact",
	align: "center middle",
	showContinue: true
});

githubGuide.register();