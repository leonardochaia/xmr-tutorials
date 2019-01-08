// Definitions credits to https://github.com/cryptoshrimpi/monerod-js/blob/master/lib/ts/monerod-js.ts

export interface GetVersion {
    status: string;
    version: number;
}

export interface GetBlockCount {
    count: number;
    status: string;
}

export interface SubmitBlock {
    status: string;
}

export interface GetBlockTemplate {
    blocktemplate_blob: string;
    difficulty: number;
    height: number;
    prev_hash: string;
    reserved_offset: number;
    status: string;
}

export interface BlockHeader {
    depth: number;
    difficulty: number;
    hash: string;
    height: number;
    major_version: number;
    minor_version: number;
    nonce: number;
    orphan_status: boolean;
    prev_hash: string;
    reward: number;
    timestamp: number;
}

export interface GetFeeEstimate {
    status: string;
    fee: number;
}

export interface GetLastBlockHeader {
    block_header: BlockHeader;
}

export interface GetHeight {
    height: number;
    status: string;
}

export type GetBlockHeaderByHash = GetLastBlockHeader;

export type GetBlockHeaderByHeight = GetLastBlockHeader;

export interface GetBlock {
    blob: string;
    block_header: BlockHeader;
    // json: GetBlockJSON.Root;
    json: any;
    status: string;
}

export interface GetConnections {
    connections: {
        avg_download: number;
        avg_upload: number;
        current_download: number;
        current_upload: number;
        incoming: boolean;
        ip: string;
        live_time: number;
        local_ip: boolean;
        localhost: boolean;
        peer_id: string;
        port: string;
        recv_count: number;
        recv_idle_time: number;
        send_count: number;
        send_idle_time: number;
        state: string;
    }[];
    status: string;
}


export interface GetInfo {
    alt_blocks_count: number;
    difficulty: number;
    grey_peerlist_size: number;
    height: number;
    incoming_connections_count: number;
    outgoing_connections_count: number;
    status: string;
    target: number;
    target_height: number;
    testnet: boolean;
    top_block_hash: string;
    tx_count: number;
    tx_pool_size: number;
    white_peerlist_size: number;
}


export interface GetHardFork {
    earliest_height: number;
    enabled: boolean;
    state: number;
    status: string;
    threshold: number;
    version: number;
    votes: number;
    voting: number;
    window: number;
}

export type BansList = { ip: string, ban: Boolean, seconds: number }[];

export interface SetBans {
    status: string;
}

export interface GetBans {
    bans: { ip: string, seconds: number }[];
    status: string;
}

export interface RequestBody { jsonrpc: string; id: string; method: string; params: any; }

export interface GetTransactions {
    status: string;
    txs_as_hex?: string;
    txs_as_json?: any;
    missed_tx?: string[];
}

export enum SpentStatus {
    unspent = 0,
    spentInBlockchain = 1,
    spentInTransactionPool = 2
}

export interface SendRawTransaction {
    double_spend: boolean;
    fee_too_low: boolean;
    invalid_input: boolean;
    invalid_output: boolean;
    low_mixin: boolean;
    not_rct: boolean;
    not_relayed: boolean;
    overspend: boolean;
    reason: string;
    status: string;
    too_big: boolean;
}

export interface IsKeyImageSpent { spent_status: SpentStatus[]; status: string; }

export interface GetTransactionPool {
    spent_key_images: {
        id_hash: string;
        txs_hashes: string[]
    }[];
    status: string;
    transactions: {
        blob_size: number;
        fee: any;
        id_hash: string;
        kept_by_block: boolean;
        last_failed_height: number;
        last_failed_id_hash: string;
        max_used_block_height: number;
        max_used_block_id_hash: string;
        receive_time: number;
        relayed: boolean;
        // tx_json: GetTransactionPoolTransaction;
        tx_json: string; // monero returns invalid json...
    }[];
}
