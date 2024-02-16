mergeInto(LibraryManager.library, {
  QuitGame: function (userName, score) {
    window.dispatchReactUnityEvent("QuitGame");
  },
});