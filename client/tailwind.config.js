const colors = require("tailwindcss/colors");

/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,ts}"],
  theme: {
    extend: {
      colors: {
        primary: colors.amber["500"],
        secondary: "#56CCF2",
        "shopping-list": "#FFF0DE",
        "add-item": "#80485B",
        "somewhat-black": "#34333A",
      },
      fontFamily: {
        quicksand: ["Quicksand", "sans-serif"],
      },
      animation: {
        "slide-in": "slide-in 0.5s ease-in-out",
        "slide-out": "slide-out 0.5s ease-in-out forwards",
      },
      keyframes: {
        "slide-in": {
          "0%": { transform: "translateX(100%)" },
          "100%": { transform: "translateX(0)" },
        },
        "slide-out": {
          "0%": { transform: "translateX(0)" },
          "99%": { transform: "translateX(100%)", opacity: 1 },
          "100%": { opacity: 0, display: "none" },
        }
      }
    },
  },
  plugins: [require("@tailwindcss/forms")],
};
