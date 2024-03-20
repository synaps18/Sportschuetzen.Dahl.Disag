﻿using Sportschuetzen.Dahl.Disag.Rm3.Enum;
using Sportschuetzen.Dahl.Disag.Rm3.Extensions;
using Sportschuetzen.Dahl.Disag.Rm3.Structs;

namespace Sportschuetzen.Dahl.Disag.Rm3.Serial;

public class SerialWrapper : IDisposable
{
    public event EventHandler<DisagResponse>? OnDataReceived
    {
        add => _serialConnection.OnDataReceived += value;
        remove => _serialConnection.OnDataReceived -= value;
    }


    public event EventHandler<EDisagHex>? OnHexReceived
    {
        add => _serialConnection.OnHexReceived += value;
        remove => _serialConnection.OnHexReceived -= value;
    }


    public event EventHandler<bool>? OnConnectionChanged
    {
        add => _serialConnection.OnConnectionChanged += value;
        remove => _serialConnection.OnConnectionChanged -= value;
    }

    private readonly SerialConnection _serialConnection;

    public SerialWrapper(string comPort)
    {
        _serialConnection = new SerialConnection(comPort);
        _serialConnection.SetBaud(EDisagBaudrate.B_38400);
    }

    public bool Connect()
    {
	    return _serialConnection.Connect();
    }

    public bool Disconnect()
    {
        return _serialConnection.Disconnect();
    }

    public async Task Send(string data)
    {
        await Send(EDisagHex.ENQ);
        var dataAsByteArray = data.ToByteArray(true);

        this.Debug("Sending STRING: " + data);
        await _serialConnection.WriteToSerial(dataAsByteArray, EDisagBaudrate.B_38400, true);
    }

    public async Task Send(EDisagBefehle command)
    {
        await Send(EDisagHex.ENQ);
        var dataAsByteArray = command.ToString().ToByteArray(true);

        this.Debug("Sending BEFEHL: " + command);
        await _serialConnection.WriteToSerial(dataAsByteArray, EDisagBaudrate.B_38400, true);
    }

    public async Task Send(int ediGesamtschusszahl, int ediSchusszahlLetzteScheibe)
    {
        await Send(EDisagHex.ENQ);
        var edi = $"{EDisagBefehle.EDI}={ediGesamtschusszahl};{ediSchusszahlLetzteScheibe}";
        var dataAsByteArray = edi.ToByteArray(true);

        this.Debug("Sending BEFEHL: " + edi);
        await _serialConnection.WriteToSerial(dataAsByteArray, EDisagBaudrate.B_38400, true);
    }

    public async Task Send(EDisagMode mode)
    {
        var stringedMode = mode.ToString();
        var dataAsByteArray = stringedMode.ToByteArray(false);

        this.Debug("Sending MODE: " + mode);
        await _serialConnection.WriteToSerial(dataAsByteArray, EDisagBaudrate.B_2400, true);
    }

    public async Task Send(EDisagHex hex)
    {
        this.Debug("Sending HEX: " + hex);
        var waitForStx = hex == EDisagHex.ENQ;

        await _serialConnection.WriteToSerial(new[] { (byte)hex }, EDisagBaudrate.B_38400, false, waitForStx);
    }

    public void Dispose()
    {
        _serialConnection.Dispose();
    }
}
