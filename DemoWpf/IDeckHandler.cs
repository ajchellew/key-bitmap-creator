using OpenMacroBoard.SDK;

namespace DemoWpf;

public interface IDeckHandler
{
    void OnKeyStateChanged(KeyEventArgs e);
}