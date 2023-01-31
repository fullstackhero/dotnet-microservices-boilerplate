const config = {
    branches: ['main'],
    plugins: [
        '@semantic-release/commit-analyzer',
        '@semantic-release/release-notes-generator',
        ["@semantic-release/git", {
            "message": "chore(release): ${nextRelease.version} [skip ci]\n\n${nextRelease.notes}"
        }],
        [
            "@semantic-release/changelog",
            {
                "changelogFile": "docs/CHANGELOG.md"
            }
        ],
        '@semantic-release/github',
        [
            "@droidsolutions-oss/semantic-release-update-file",
            {
                "files": [
                    {
                        "path": ["Directory.Build.props"],
                        "type": "xml",
                        "replacements": [{ "key": "Version", "value": "${nextRelease.version}" }, { "key": "ContainerImageTags", "value": "${nextRelease.version};latest" }]
                    }
                ]
            }
        ],
    ]
};

module.exports = config;