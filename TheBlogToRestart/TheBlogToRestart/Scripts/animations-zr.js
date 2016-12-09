var controller = new ScrollMagic.Controller();

// build scene
var scene = new ScrollMagic.Scene({ triggerElement: "#mostRecentContent" })
                // trigger a velocity opaticy animation
                .setVelocity("#wine", { opacity: 0 }, { duration: 400 })
                .addIndicators() // add indicators (requires plugin)
                .addTo(controller);