export default function getStyle(data) {
    return {
        container: {
            width: data.width,
            height: data.height,
            // borderRadius: 12,
            // paddingLeft: data.width * 0.03,
            // paddingRight: data.width * 0.03,
        },
        rankList: {
            width: data.width,
            height: data.height,
        },
        list: {
            width: data.width,
            height: data.height,
        },
        listItem: {
            position: 'relative',
            width: data.width,
            height: data.height / 2 / 4,
            flexDirection: 'row',
            alignItems: 'center',
            marginTop: 2,
        },

        listItemEven: {
            backgroundColor: '#435472',
        },

        listItemNum: {
            fontSize: data.width * 0.043,
            fontWeight: 'bold',
            lineHeight: (data.height / 2 / 4) * 0.7,
            textAlign: 'center',
            width: (data.height / 2 / 4) * 0.7,
            height: (data.height / 2 / 4) * 0.7,
            color: '#fff',
        },

        rankAvatar: {
            // borderRadius: data.width * 0.06,
            marginLeft: data.width * 0.01,
            width: (data.height / 2 / 4) * 0.6,
            height: (data.height / 2 / 4) * 0.6,
        },
        rankName: {
            marginLeft: data.width * 0.01,
            width: data.width * 0.2,
            height: (data.height / 2 / 4) * 0.4,
            textAlign: 'center',
            lineHeight: (data.height / 2 / 4) * 0.4,
            fontSize: data.width * 0.043,
            textOverflow: 'ellipsis',
            color: '#fff',
        },

        listScoreUnit: {
            marginLeft: data.width * 0.2,
            fontSize: data.width * 0.043,
            height: (data.height / 2 / 4) * 0.4,
            lineHeight: (data.height / 2 / 4) * 0.4,
            width: data.width * 0.15,
            color: '#fff',
        },

        listItemScore: {
            marginLeft: data.width * 0.02,
            width: data.width * 0.2,
            height: (data.height / 2 / 4) * 0.3,
            lineHeight: (data.height / 2 / 4) * 0.3,
            fontSize: data.width * 0.043,
            color: '#fff',
        }
    };
}
