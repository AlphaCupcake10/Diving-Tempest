mergeInto(LibraryManager.library, {
  SetTime: function (recordTime) {
    try {
      window.dispatchReactUnityEvent("SetTime", recordTime);
    } catch (e) {
      console.warn("Failed to dispatch event");
    }
  },
  QuitGame: function (returnCode) {
    try {
      window.dispatchReactUnityEvent("QuitGame",returnCode);
    } catch (e) {
      console.warn("Failed to dispatch event");
    }
  },
});