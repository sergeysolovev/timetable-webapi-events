export default function(name)
{
  const re = new RegExp(name + "=([^;]+)");
  const value = re.exec(document.cookie);
  return (value != null) ? unescape(value[1]) : null;
}
