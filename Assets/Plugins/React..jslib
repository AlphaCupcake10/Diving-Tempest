mergeInto(LibraryManager.library, {
  QuitGame: function () {
    window.dispatchReactUnityEvent("QuitGame");
  },
});