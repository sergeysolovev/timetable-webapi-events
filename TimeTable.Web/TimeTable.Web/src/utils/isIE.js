export function isIE11orAbove() {
  return navigator.userAgent.indexOf("Trident") !== -1;
}

export function isIE10() {
  return navigator.appVersion.indexOf("MSIE 10") !== -1;
}

export function isIE9orBelow() {
  return /MSIE\s/.test(navigator.userAgent) && parseFloat(navigator.appVersion.split("MSIE")[1]) < 10;
}
