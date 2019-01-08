import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import {
  RequestBody, GetBlockCount,
  GetBlockTemplate, GetLastBlockHeader,
  GetBlock, GetConnections, GetInfo,
  GetHardFork, SetBans, GetBans, GetHeight,
  GetTransactions, IsKeyImageSpent,
  SendRawTransaction, GetTransactionPool,
  SubmitBlock, GetVersion, GetFeeEstimate,
  GetBlockHeaderByHash,
  GetBlockHeaderByHeight, BansList
} from './daemon-client.models';

/**
 * Monero Daemon RPC Client
 * Inspired on https://github.com/cryptoshrimpi/monerod-js/blob/master/lib/ts/monerod-js.ts
 */
@Injectable()
export class DaemonClientService {

  constructor(
    @Inject('MONEROD_HOSTNAME')
    private readonly hostname: string,
    private readonly http: HttpClient) {
  }

  public getBlockCount() {
    const body = this.buildDefaultRequestBody('getblockcount', null);
    return this.defaultRequest<GetBlockCount>(body);
  }

  public onGetBlockHash(blockHeight: number) {
    const body = this.buildDefaultRequestBody('on_getblockhash', [blockHeight]);
    return this.defaultRequest<string>(body);
  }

  public getBlockTemplate(walletAddress: string, reserveSize: number) {
    const body = this.buildDefaultRequestBody('getblocktemplate', {
      'wallet_address': walletAddress,
      'reserve_size': reserveSize
    });
    return this.defaultRequest<GetBlockTemplate>(body);
  }

  public getLastBlockHeader() {
    const body = this.buildDefaultRequestBody('getlastblockheader', null);
    return this.defaultRequest<GetLastBlockHeader>(body);
  }

  public getBlockHeaderByHash(hash: string) {
    const body = this.buildDefaultRequestBody('getblockheaderbyhash', {
      'hash': hash
    });
    return this.defaultRequest<GetBlockHeaderByHash>(body);
  }

  public getBlockHeaderByHeight(height: number) {
    const body = this.buildDefaultRequestBody('getblockheaderbyheight', {
      'height': height
    });
    return this.defaultRequest<GetBlockHeaderByHeight>(body);
  }

  public getBlock(height: number, hash: string) {
    const body = this.buildDefaultRequestBody('getblock', {
      'height': height,
      'hash': hash
    });
    return this.defaultRequest<GetBlock>(body);
    // return new Promise((resolve, reject) => {
    //     this.defaultRequest(body).then((a) => {
    //         a.json = JSON.parse(a.json);
    //         resolve(a);
    //     }).catch((f) => {
    //         reject(f);
    //     });
    // }) as GetBlockPromise;
  }

  public getConnections() {
    const body = this.buildDefaultRequestBody('get_connections', null);
    return this.defaultRequest<GetConnections>(body);
  }

  public getInfo() {
    const body = this.buildDefaultRequestBody('get_info', null);
    return this.defaultRequest<GetInfo>(body);
  }

  public getHardFork() {
    const body = this.buildDefaultRequestBody('hard_fork_info', null);
    return this.defaultRequest<GetHardFork>(body);
  }

  public setBans(bans: BansList) {
    const body = this.buildDefaultRequestBody('setbans', { 'bans': bans });
    return this.defaultRequest<SetBans>(body);
  }

  public getBans() {
    const body = this.buildDefaultRequestBody('getbans', null);
    return this.defaultRequest<GetBans>(body);
  }

  public getHeight() {
    return this.request<GetHeight>(null, '/getheight');
  }

  public getTransactions(txsHashes: string[], decodeAsJson: boolean) {
    const body = { txs_hashes: txsHashes, decode_as_json: decodeAsJson };

    return this.request<GetTransactions>(body as any, '/gettransactions')
      .then((a) => {
        if (decodeAsJson && a.hasOwnProperty('txs_as_json')) {
          a.txs_as_json = JSON.parse(a.txs_as_json);
        }
        return a;
      });
  }

  public isKeyImageSpent(keyImages: string[]) {
    return this.request<IsKeyImageSpent>({ key_images: keyImages }, '/is_key_image_spent');
  }

  public sendRawTransaction(txAsHex: string) {
    return this.request<SendRawTransaction>({ tx_as_hex: txAsHex }, '/sendrawtransaction');
  }

  public getTransactionPool() {
    return this.request<GetTransactionPool>({}, '/get_transaction_pool');
    /*
    Monero returns invalid JSON
    for (var key in a.transactions) {
        a.transactions[key].tx_json = JSON.parse(a.transactions[key].tx_json);
    }*/
  }

  public submitBlock(blockBlobData: string) {
    const body = this.buildDefaultRequestBody('submitblock', [blockBlobData]);
    return this.defaultRequest<SubmitBlock>(body);
  }

  public getVersion() {
    const body = this.buildDefaultRequestBody('get_version', null);
    return this.defaultRequest<GetVersion>(body);
  }

  public getFeeEstimate() {
    const body = this.buildDefaultRequestBody('get_fee_estimate', null);
    return this.defaultRequest<GetFeeEstimate>(body);
  }

  private defaultRequest<TResult>(requestBody: RequestBody): Promise<TResult> {
    return this.request<TResult>(requestBody, '/json_rpc');
  }

  private request<TResult>(requestBody: any, path: String): Promise<TResult> {
    return this.http.post<{ result: TResult }>(this.hostname + path, requestBody)
      .pipe(map(r => r.result))
      .toPromise();
  }

  private buildDefaultRequestBody(method: string, params: any): RequestBody {
    return {
      jsonrpc: '2.0',
      id: '0',
      method: method,
      params: params
    };
  }

}
